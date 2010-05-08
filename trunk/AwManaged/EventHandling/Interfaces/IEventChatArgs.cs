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
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventChatArgs<TAvatar> where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
    {
        ChatType ChatType { get; }
        TAvatar Avatar { get; }
        string Message { get; }
    }
}