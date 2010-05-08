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

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void AvatarEventRemoveDelegate(AwManaged.BotEngine sender, EventAvatarRemoveArgs e);

    /// <summary>
    /// This event gets fired when an avatar is removed from the world list.
    /// </summary>
    public sealed class EventAvatarRemoveArgs : MarshalIndefinite
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarRemoveArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}