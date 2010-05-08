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
using AwManaged.Core.Scheduling.Interfaces;

namespace AwManaged.Core.Scheduling
{
    /// <summary>
    /// Helper struct that simplifies interval configuration.
    /// </summary>
    public class SchedulingIntervals : ISchedulingIntervals
    {

        private readonly SchedulingApi schedule;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SchedulingIntervals(SchedulingApi schedule)
        {
            this.schedule = schedule;
        }

        /// <summary>
        /// Sets an interval of a given number of milliseconds.
        /// </summary>
        /// <param name="value">The interval in milliseconds.</param>
        public SchedulingApi Milliseconds(long value)
        {
            return schedule.EveryInternal(System.TimeSpan.FromMilliseconds(value));
        }


        /// <summary>
        /// Sets an interval of a given number of seconds.
        /// </summary>
        /// <param name="value">The interval in seconds.</param>
        public SchedulingApi Seconds(double value)
        {
            return schedule.EveryInternal(System.TimeSpan.FromSeconds(value));
        }


        /// <summary>
        /// Sets an interval of a given number of minutes.
        /// </summary>
        /// <param name="value">The interval in minutes.</param>
        public SchedulingApi Minutes(double value)
        {
            return schedule.EveryInternal(System.TimeSpan.FromMinutes(value));
        }

        public SchedulingApi Hours(double value)
        {
            return schedule.EveryInternal(System.TimeSpan.FromHours(value));
        }
    

        /// <summary>
        /// Sets an interval of a given number of days.
        /// </summary>
        /// <param name="value">The interval in days.</param>
        public SchedulingApi Days(double value)
        {
            return schedule.EveryInternal(System.TimeSpan.FromDays(value));
        }


        /// <summary>
        /// Sets an interval of a given time span.
        /// </summary>
        /// <param name="value">The interval.</param>
        public SchedulingApi TimeSpan(TimeSpan value)
        {
            return schedule.EveryInternal(value);
        }


    }
}