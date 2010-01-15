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
using AwManaged.Core.Scheduling.Interfaces;

namespace AwManaged.Core.Scheduling
{
    public class SchedulingApi : ISchedulingApi
    {
        private readonly SchedulingItem _schedulingItem;

        public SchedulingApi(SchedulingItem schedulingItem)
        {
            _schedulingItem = schedulingItem;
        }

        public SchedulingApi From(DateTimeOffset startTime)
        {
            _schedulingItem.StartTime = startTime;
            return this;
        }

        public SchedulingApi Once()
        {
            _schedulingItem.Loops = 1;
            _schedulingItem.ExpirationTime = null;
            _schedulingItem.Interval = null;
            return this;
        }

        internal SchedulingApi EveryInternal(TimeSpan interval)
        {
            _schedulingItem.Interval = interval;
            return this;
        }

        public SchedulingIntervals Every
        {
            get { return new SchedulingIntervals(this);  }
        }

        public SchedulingApi Times(int loops)
        {
            _schedulingItem.Loops = loops;
            return this;
        }

        public SchedulingApi Until(DateTimeOffset jobExpirationTime)
        {
            _schedulingItem.ExpirationTime = jobExpirationTime;
            return this;
        }
    }
}