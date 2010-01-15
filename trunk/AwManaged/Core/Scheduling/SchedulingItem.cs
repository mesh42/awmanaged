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
using AwManaged.Core.Scheduling.Enumerations;
using AwManaged.Core.Scheduling.Interfaces;

namespace AwManaged.Core.Scheduling
{
    /// <summary>
    /// Encapsulates the scheduling for a given job.
    /// </summary>
    public class SchedulingItem : ISchedulingItem
    {
        /// <summary>
        /// A unique identifier for the job.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets a synchronization token for the job.
        /// </summary>
        public object SynchronizationToken
        {
            get { return this; }
        }

        /// <summary>
        /// The designated start time for the job. If not set, the
        /// job starts immediately.
        /// </summary>
        public virtual DateTimeOffset StartTime { get; set; }

        private TimeSpan? interval;

        /// <summary>
        /// The interval of the job. Required if the job is to supposed multiple
        /// times.
        /// </summary>
        public virtual TimeSpan? Interval
        {
            get { return interval; }
            set
            {
                if (interval.HasValue && interval.Value.TotalMilliseconds < 0)
                {
                    string msg = "Invalid interval of {0} milliseconds. Interval must be a positive value or [null].";
                    msg = String.Format(msg, interval.Value.TotalMilliseconds);
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SynchronizationToken)
                {
                    interval = value;
                }
            }
        }

        private int? loops;

        /// <summary>
        /// The number of executed loops. If set, the
        /// job runs the specified number of times, unless
        /// it aborts because the <see cref="ExpirationTime"/>
        /// was set and causes the job to be cancelled earlier.
        /// </summary>
        public virtual int? Loops
        {
            get { return loops; }
            set
            {
                if (value.HasValue && value < 1)
                {
                    string msg = "Invalid number of loops: {0}. Only numbers above zero or [null] are allowed.";
                    msg = String.Format(msg, value);
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SynchronizationToken)
                {
                    loops = value;
                }
            }
        }


        private DateTimeOffset? expirationTime;

        /// <summary>
        /// The expiration time of the job. This date is optional
        /// in case the job runs only a specified number of times,
        /// or indefinitely.
        /// </summary>
        public virtual DateTimeOffset? ExpirationTime
        {
            get { return expirationTime; }
            set
            {
                if (value.HasValue && value < SchedulingTimeHelpers.Now())
                {
                    const string msg = "Expiration time cannot be in the past";
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SynchronizationToken)
                {
                    expirationTime = value;
                }
            }
        }

        /// <summary>
        /// The job's current state.
        /// </summary>
        public virtual SchedulingItemStateType SchedulingState { get; protected internal set; }


        /// <summary>
        /// Creates a new job, and generates a unique job ID on the fly.
        /// </summary>
        public SchedulingItem() : this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// Initializes a new job with a given identifier.
        /// </summary>
        /// <param name="jobId">A unique ID for the job. Unique
        /// job IDs are not evaluated by the scheduler, the calling
        /// party must ensure unique IDs.</param>
        public SchedulingItem(string jobId)
        {
            Id = jobId;
        }


        /// <summary>
        /// Configures how often the job is being repeated.
        /// </summary>
        public virtual SchedulingApi Run
        { 
            get { return new SchedulingApi(this); }
        }

        /// <summary>
        /// Cancels the job in order to have it removed
        /// during the next processing loop. This method
        /// can be invoked by clients in order to
        /// cancel job execution without having to reference
        /// the job's <see cref="Scheduler"/>.
        /// </summary>
        public virtual void Cancel()
        {
            lock (this)
            {
                SchedulingState = SchedulingItemStateType.Aborted;
            }
        }


        /// <summary>
        /// Pauses the current job.
        /// </summary>
        /// <returns>True if the job's <see cref="State"/>
        /// was <see cref="JobState.Active"/> and was changed
        /// to <see cref="JobState.Paused"/>. False if the job's
        /// <see cref="State"/> is not <see cref="JobState.Active"/>.</returns>
        /// <exception cref="InvalidOperationException">If the job has no
        /// interval, and can thus not be rescheduled. For a job that runs
        /// just once, set the <see cref="StartTime"/> accordingly.</exception>
        public virtual bool Pause()
        {
            if(!Interval.HasValue)
            {
                const string msg = "Jobs without interval cannot be paused " 
                                   + "- the scheduler does not know how to schedule it once it is resumed.";
                throw new InvalidOperationException(msg);
            }

            lock (this)
            {
                if (SchedulingState != SchedulingItemStateType.Running) return false;
                SchedulingState = SchedulingItemStateType.Pause;
                return true;
            }
        }

        /// <summary>
        /// Resumes a paused job.
        /// </summary>
        /// <returns>True if the job's <see cref="State"/>
        /// was <see cref="JobState.Paused"/> and was changed
        /// to <see cref="JobState.Active"/>. False if the job's
        /// <see cref="State"/> is not <see cref="JobState.Paused"/>.</returns>
        public virtual bool Continue()
        {
            lock (this)
            {
                if (SchedulingState != SchedulingItemStateType.Pause) return false;
                SchedulingState = SchedulingItemStateType.Running;
                return true;
            }
        }
    }

    /// <summary>
    /// A generic job implementation which allows to attach
    /// strongly typed state information directly to the
    /// job.
    /// </summary>
    /// <typeparam name="T">The type of the job's
    /// <see cref="Data"/> object.</typeparam>
    public class Job<T> : SchedulingItem
    {
        /// <summary>
        /// Gets or sets the state object that is attached to the job.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Creates a new job, and generates a unique job identifier on the fly.
        /// </summary>
        public Job()
        {
        }

        /// <summary>
        /// Initializes a new job with a given identifier.
        /// </summary>
        public Job(string id) : base(id)
        {
        }
    }
}