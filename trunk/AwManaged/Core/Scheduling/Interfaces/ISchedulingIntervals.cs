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
    public interface ISchedulingIntervals
    {
        /// <summary>
        /// Sets an interval of a given number of milliseconds.
        /// </summary>
        /// <param name="value">The interval in milliseconds.</param>
        SchedulingApi Milliseconds(long value);

        /// <summary>
        /// Sets an interval of a given number of seconds.
        /// </summary>
        /// <param name="value">The interval in seconds.</param>
        SchedulingApi Seconds(double value);

        /// <summary>
        /// Sets an interval of a given number of minutes.
        /// </summary>
        /// <param name="value">The interval in minutes.</param>
        SchedulingApi Minutes(double value);

        SchedulingApi Hours(double value);

        /// <summary>
        /// Sets an interval of a given number of days.
        /// </summary>
        /// <param name="value">The interval in days.</param>
        SchedulingApi Days(double value);

        /// <summary>
        /// Sets an interval of a given time span.
        /// </summary>
        /// <param name="value">The interval.</param>
        SchedulingApi TimeSpan(TimeSpan value);
    }
}