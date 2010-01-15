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
using System;
using System.Threading;
using AwManaged.Core.Scheduling.Enumerations;

namespace AwManaged.Core.Scheduling
{
    /// <summary>
    /// Contains the context of a managed job. Internally used by
    /// the scheduler.
    /// </summary>
    public class SchedulingItemContext
    {
        /// <summary>
        /// The callback action that is being invoked.
        /// </summary>
        public Action<SchedulingItem> CallbackAction { get; protected set; }

        /// <summary>
        /// The maintained job.
        /// </summary>
        public SchedulingItem ManagedJob { get; set; }

        /// <summary>
        /// The timestamp of the last job execution. Returns null
        /// if the job hasn't been executed yet.
        /// </summary>
        public DateTimeOffset? LastJobEvaluation { get; set; }

        /// <summary>
        /// The next execution time. Returns null if the job is done.
        /// After initialization, this is the job's <see cref="Job.StartTime"/>.
        /// </summary>
        public DateTimeOffset? NextExecution { get; set; }

        /// <summary>
        /// The remaining executions, if the job was configured
        /// to run a specific number of times.
        /// </summary>
        public int? RemainingExecutions { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">If one of the parameters is
        /// is a null reference.</exception>
        /// <exception cref="InvalidOperationException">If the configuration does not allow
        /// proper scheduling, e.g. because several loops were specified, but the inverval is
        /// missing.</exception>
        public SchedulingItemContext(SchedulingItem managedJob, Action<SchedulingItem> callbackAction)
        {
            if (managedJob == null) throw new ArgumentNullException("managedJob");
            if (callbackAction == null) throw new ArgumentNullException("callbackAction");

            //make sure we have an interval if we have more than one loop
            TimeSpan? interval = managedJob.Interval;
            if (interval == null)
            {
                if (managedJob.Loops == null || managedJob.Loops.Value > 1)
                {
                    string msg = "Job [{0}] is invalid: Specifiy either a single run, or a loop interval.";
                    msg = String.Format(msg, managedJob.Id);
                    throw new InvalidOperationException(msg);
                }
            }

            ManagedJob = managedJob;
            CallbackAction = callbackAction;
            NextExecution = managedJob.StartTime;

            //adjust starting time if the initial time was in the past,
            //but rather start immediately
            var now = SchedulingTimeHelpers.Now();
            if (NextExecution.Value < now) NextExecution = now;

            RemainingExecutions = managedJob.Loops;
        }



        /// <summary>
        /// Invokes the managed job's <see cref="CallbackAction"/> through
        /// the thread pool, and updates the job's internal state.
        /// </summary>
        public virtual void ExecuteAsync(SchedulingService scheduler)
        {
            //only execute if the job is neither aborted, expired, or paused
            if (ManagedJob.SchedulingState == SchedulingItemStateType.Running && (ManagedJob.ExpirationTime == null || ManagedJob.ExpirationTime >= SchedulingTimeHelpers.Now()))
            {
                ThreadPool.QueueUserWorkItem(s =>
                                                 {
                                                     try
                                                     {
                                                         CallbackAction(ManagedJob);
                                                     }
                                                     catch (Exception e)
                                                     {
                                                         //do not reference the scheduler instance to avoid closure
                                                         SchedulingService sch = (SchedulingService) s;
                                                         if(!sch.SubmitJobException(ManagedJob, e)) throw;
                                                     }
                                                 }, scheduler);
            }

            UpdateState();
        }


        /// <summary>
        /// Updates the internal state after an execution, and updates the
        /// <see cref="LastJobEvaluation"/> and <see cref="NextExecution"/>
        /// values. If the job is not supposed to run anymore, the
        /// <see cref="NextExecution"/> property is set to null.
        /// </summary>
        protected virtual void UpdateState()
        {
            LastJobEvaluation = SchedulingTimeHelpers.Now();

            lock (ManagedJob.SynchronizationToken)
            {
                if (ManagedJob.SchedulingState == SchedulingItemStateType.Aborted)
                {
                    NextExecution = null;
                    return;
                }


                if (RemainingExecutions.HasValue)
                {
                    //only decrease the loop counter if the job is not paused
                    if (ManagedJob.SchedulingState == SchedulingItemStateType.Running)
                    {
                        RemainingExecutions--;
                    }

                    //we're done if we peformend the last loop
                    if (RemainingExecutions == 0)
                    {
                        //we have a loop, and completed it
                        ManagedJob.SchedulingState = SchedulingItemStateType.Done;
                        NextExecution = null;
                        return;
                    }
                }


                //if there is no reoccurrence interval, we cannot calculate a new run
                //-> cancel
                if (!ManagedJob.Interval.HasValue)
                {
                    ManagedJob.SchedulingState = SchedulingItemStateType.Aborted;
                    NextExecution = null;
                    return;
                }

                //schedule next execution - even if this is beyond expiration
                //(we don't cancel as long as expiration happened)
                NextExecution = (NextExecution.Value).Add(ManagedJob.Interval.Value);

                //if the next job is beyond expiration, reset again
                if (ManagedJob.ExpirationTime.HasValue && LastJobEvaluation.Value > ManagedJob.ExpirationTime)
                {
                    ManagedJob.SchedulingState = SchedulingItemStateType.Done;
                    NextExecution = null;
                }
            }
        }
    }
}