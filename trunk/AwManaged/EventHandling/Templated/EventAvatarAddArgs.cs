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
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void AvatarEventAddDelegate<TSender, TAvatar>(TSender sender, Templated.EventAvatarAddArgs<TAvatar> e)
    where TAvatar : MarshalIndefinite, IAvatar<TAvatar>;

    public sealed class EventAvatarAddArgs<TAvatar> : MarshalIndefinite, IEventAvatarAddArgs<TAvatar>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
    {
        public TAvatar Avatar { get; private set; }

        public EventAvatarAddArgs(ICloneableT<TAvatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}