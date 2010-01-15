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

namespace AwManaged.Core.Scheduling.Interfaces
{
    public interface ISchedulingItem
    {
        string Id { get; set; }
        object SynchronizationToken { get; }
        DateTimeOffset StartTime { get; set; }
        TimeSpan? Interval { get; set; }
        int? Loops { get; set; }
        DateTimeOffset? ExpirationTime { get; }
        SchedulingItemStateType SchedulingState { get; }
        SchedulingApi Run { get; }
        void Cancel();
        bool Pause();
        bool Continue();
    }
}