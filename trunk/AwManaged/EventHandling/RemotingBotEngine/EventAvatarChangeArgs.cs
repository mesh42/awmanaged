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
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void AvatarEventChangeDelegate(RemoteServices.RemotingBotEngine sender, EventAvatarChangeArgs e);

    public class EventAvatarChangeArgs : MarshalIndefinite
    {
        public Avatar AvatarPreviousState
        {
            get; private set;
        }

        public Avatar Avatar
        {
            get; private set;
        }

        public EventAvatarChangeArgs(ICloneableT<Avatar> avatar, ICloneableT<Avatar> avatarPreviousState)
        {
            Avatar = avatar.Clone();
            AvatarPreviousState = avatarPreviousState.Clone();
        }
    }
}