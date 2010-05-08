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
using AwManaged.Core.Scheduling.Enumerations;
using AwManaged.Core.Services.Interfaces;

namespace AwManaged.Core.Scheduling.Interfaces
{
    public interface ISchedulingService : IService
    {
        ReschedulingType ReschedulingType { get; set; }
        Action<SchedulingItem, Exception> SchedulingItemExceptionHandler { get; set; }
        int SelfTestInterval { get; set; }
        int MinJobInterval { get; set; }
        object SyncRoot { get; }
        DateTimeOffset? NextExecution { get; }
        void Submit<T>(Job<T> job, Action<Job<T>, T> callback);
        void Submit(SchedulingItem job, Action<SchedulingItem> callback);
        SchedulingItem TryGet(string id);
        bool Pause(string id);
        bool Resume(string id);
        bool Cancel(string id);
        void CancelAll();
    }
}