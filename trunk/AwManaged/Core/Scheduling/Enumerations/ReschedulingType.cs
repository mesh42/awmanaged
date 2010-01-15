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
namespace AwManaged.Core.Scheduling.Enumerations
{
    public enum ReschedulingType
    {
        /// <summary>
        /// The scheduled item will not be rescheduled if a  system time change was detected.
        /// </summary>
        IgnoreSystemTimeChanged,
        /// <summary>
        /// Only the jobs' next execution time is rescheduled.
        /// The expiration time remains fixed.
        /// </summary>
        RescheduleNextExecution,
        /// <summary>
        /// Both the jobs' next execution times and the
        /// expiration times are being shifted according
        /// to the time change.
        /// </summary>
        RescheduleNextExecutionAndExpirationTime
    }
}