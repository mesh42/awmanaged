/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using SharedMemory;using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AwManaged.Core.Scheduling.Enumerations;
using AwManaged.Core.Services;

namespace AwManaged.Core.Scheduling
{
    public class SchedulingService : BaseService
    {
        protected long LastTimerInterval { get; set; }
        protected DateTimeOffset LastTimerRun { get; private set; }
        protected bool IsDisposed { get; set; }
        public ReschedulingType ReschedulingType { get; set; }
        public Action<SchedulingItem, Exception> SchedulingItemExceptionHandler { get; set; }
        private int _selfTestInterval = 120000;
        private int _minJobInterval = 100;
        protected Timer Timer { get; set; }
        protected List<SchedulingItemContext> SchedulingItems { get; set; }
        private readonly object _objectToken = new object();

        public int SelfTestInterval
        {
            get { return _selfTestInterval; }
            set
            {
                if (value < 100)
                {
                    const string msg = "Self testing interval must be greater than 100 milliseconds.";
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                _selfTestInterval = value;
                lock(SyncRoot)
                {
                    Reschedule();
                }

            }
        }

        public int MinJobInterval
        {
            get { return _minJobInterval; }
            set
            {
                if(value < 1)
                {
                    const string msg = "Interval must be greater than zero milliseconds.";
                    throw new ArgumentOutOfRangeException("value", msg);
                }
                _minJobInterval = value;
            }
        }

        public object SyncRoot
        {
            get { return _objectToken; }
        }
        protected bool IsSorted { get; set; }
        public DateTimeOffset? NextExecution { get; protected set; }

        public SchedulingService()
        {
            SchedulingItems = new List<SchedulingItemContext>();
            Timer = new Timer(ProcessJobs, null, Timeout.Infinite, Timeout.Infinite);
        }

        public virtual void Submit<T>(Job<T> schedulingItem, Action<Job<T>, T> callback)
        {
            Action<SchedulingItem> action = j =>
                                     {
                                         var genericJob = (Job<T>) j;
                                         callback(genericJob, genericJob.Data);
                                     };

            Submit(schedulingItem, action);
        }

        public virtual void Submit(SchedulingItem schedulingItem, Action<SchedulingItem> callback)
        {
            if (schedulingItem == null) throw new ArgumentNullException("schedulingItem");
            if (callback == null) throw new ArgumentNullException("callback");
            var interval = schedulingItem.Interval;
            if (interval.HasValue && interval.Value.TotalMilliseconds < MinJobInterval)
            {
                string msg = "Interval of {0} ms is too small - a minimum interval of {1} ms is accepted.";
                msg = String.Format(msg, interval.Value.TotalMilliseconds, MinJobInterval);
                throw new InvalidOperationException(msg);
            }
            var context = new SchedulingItemContext(schedulingItem, callback);
            lock(SyncRoot)
            {
                if (NextExecution == null || context.NextExecution <= NextExecution.Value)
                {
                    SchedulingItems.Insert(0, context);
                    if (NextExecution == null || NextExecution.Value.Subtract(SchedulingTimeHelpers.Now()).TotalMilliseconds > MinJobInterval)
                    {
                        Reschedule();
                    }
                }
                else
                {
                    SchedulingItems.Add(context);
                    IsSorted = false;
                }
            }
        }
        public virtual SchedulingItem TryGet(string jobId)
        {
            lock(SyncRoot)
            {
                var context = SchedulingItems.FirstOrDefault(jc => jc.ManagedJob.Id == jobId);
                return context == null ? null : context.ManagedJob;
            }
        }

        public virtual bool Pause(string id)
        {
            var job = TryGet(id);
            return job == null ? false : job.Pause();
        }

        public virtual bool Resume(string id)
        {
            var job = TryGet(id);
            return job == null ? false : job.Continue();
        }

        public virtual bool Cancel(string id)
        {
            lock (SyncRoot)
            {
                for (int i = 0; i < SchedulingItems.Count; i++)
                {
                    var job = SchedulingItems[i];
                    if(job.ManagedJob.Id == id)
                    {
                        SchedulingItems.RemoveAt(i);
                        job.ManagedJob.Cancel();
                        if(i == 0) Reschedule();

                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void CancelAll()
        {
            lock(SyncRoot)
            {
                if(IsDisposed) return;

                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                NextExecution = null;
                SchedulingItems.Clear();
            }
        }

        protected virtual void ProcessJobs(object state)
        {
            lock (SyncRoot)
            {
                if (IsDisposed) return;

                if(ReschedulingType != ReschedulingType.IgnoreSystemTimeChanged)
                {
                    VerifySystemTime();
                }
                RunPendingJobs();
                if (!IsSorted) Sort();
                Reschedule();
            }
        }

        protected virtual void VerifySystemTime()
        {
            if(LastTimerInterval == Timeout.Infinite) return;
            var now = SchedulingTimeHelpers.Now();
            var pauseDuration = now.Subtract(LastTimerRun);
            var timeDivergence = pauseDuration.TotalMilliseconds - LastTimerInterval;
            if(timeDivergence > 1000 || timeDivergence < 1000)
            {
                bool changeExpirationTime = ReschedulingType ==
                                            ReschedulingType.RescheduleNextExecutionAndExpirationTime;
                SchedulingItems.ForEach(jc =>
                                 {
                                     jc.NextExecution = jc.NextExecution.Value.AddMilliseconds(timeDivergence);
                                     if(changeExpirationTime && jc.ManagedJob.ExpirationTime.HasValue)
                                     {
                                         jc.ManagedJob.ExpirationTime = jc.ManagedJob.ExpirationTime.Value.AddMilliseconds(timeDivergence);
                                     }
                                 });
            }

        }
        protected void Sort()
        {
            SchedulingItems.Sort((first, second) => first.NextExecution.Value.CompareTo(second.NextExecution.Value));
            IsSorted = true;
        }

        protected virtual void RunPendingJobs()
        {
            var now = SchedulingTimeHelpers.Now();
            var dueJobs = new List<SchedulingItemContext>();
            SchedulingItemContext currentJob = null;

            for (int i = 0; i < SchedulingItems.Count; i++)
            {
                var job = SchedulingItems[i];
                if (job.NextExecution.Value > now) break;
                dueJobs.Add(job);
            }

            try
            {
                for (var i = dueJobs.Count-1; i >=0; i--)
                {
                    currentJob = dueJobs[i];
                    currentJob.ExecuteAsync(this);
                    if (currentJob.NextExecution.HasValue)
                    {
                        IsSorted = false;
                    }
                    else
                    {
                        SchedulingItems.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                if(!SubmitJobException(currentJob.ManagedJob, e)) throw;
            }
        }

        protected virtual void Reschedule()
        {
            if(IsDisposed) return;

            if(SchedulingItems.Count == 0)
            {
                NextExecution = null;
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                LastTimerInterval = Timeout.Infinite;
            }
            else
            {
                var executionTime = SchedulingItems[0].NextExecution;
                DateTimeOffset now = SchedulingTimeHelpers.Now();
                TimeSpan delay = executionTime.Value.Subtract(now);
                long dueTime = System.Math.Max(MinJobInterval, (long)delay.TotalMilliseconds);
                dueTime = System.Math.Min(dueTime, SelfTestInterval);
                NextExecution = SchedulingTimeHelpers.Now().AddMilliseconds(dueTime);
                Timer.Change(dueTime, Timeout.Infinite);
                LastTimerInterval = dueTime;
            }

            LastTimerRun = SchedulingTimeHelpers.Now();
        }

        public override void Dispose()
        {
            lock(SyncRoot)
            {
                if(IsDisposed) return;

                IsDisposed = true;
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                Timer.Dispose();
            }
        }

        internal virtual bool SubmitJobException(SchedulingItem job, Exception exception)
        {
            var handler = SchedulingItemExceptionHandler;
            if (handler != null)
            {
                handler(job, exception);
                return true;
            }

            return false;
        }

        #region IIdentifiable Members

        public override string IdentifyableDisplayName
        {
            get { return "Scheduling Service"; }
        }

        public override Guid IdentifyableId
        {
            get { return new Guid("{7341CA72-100F-427f-9C0F-22766994AB08}"); }
        }

        public override string IdentifyableTechnicalName
        {
            get { return "schedulingsvc"; }
        }

        #endregion
    }
}