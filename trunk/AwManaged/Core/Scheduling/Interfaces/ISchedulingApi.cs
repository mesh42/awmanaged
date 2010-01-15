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

namespace AwManaged.Core.Scheduling.Interfaces
{
    public interface ISchedulingApi
    {
        /// <summary>
        /// Sets the job's starting time.
        /// </summary>
        /// <param name="startTime">The first execution of the job.</param>
        SchedulingApi From(DateTimeOffset startTime);

        /// <summary>
        /// Configures the job to run only once.
        /// </summary>
        SchedulingApi Once();

        /// <summary>
        /// Defines an interval for periodic execution of the
        /// job. The interval needs to be set if the job is
        /// supposed to run more than once.
        /// </summary>
        SchedulingIntervals Every { get; }

        /// <summary>
        /// Configures the job to run a fixed number of times.
        /// </summary>
        /// <param name="loops">The number of times the job
        /// is supposed to run.</param>
        SchedulingApi Times(int loops);

        /// <summary>
        /// Specifies an <see cref="Job.ExpirationTime"/> for the job. Not
        /// required it the job runs only a number of times, or is supposed
        /// to run indefinitely with the specified <see cref="Job.Interval"/>.
        /// </summary>
        /// <param name="jobExpirationTime">The specified expiration time.</param>
        SchedulingApi Until(DateTimeOffset jobExpirationTime);
    }
}