﻿/* **********************************************************************************
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
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void AvatarEventAddDelegate(RemoteServices.RemotingBotEngine sender, EventAvatarAddArgs e);

    public sealed class EventAvatarAddArgs : MarshalIndefinite
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarAddArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}