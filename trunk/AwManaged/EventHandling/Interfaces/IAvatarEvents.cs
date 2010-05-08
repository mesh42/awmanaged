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
using AwManaged.EventHandling.Templated;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    /// <summary>
    /// Interface which exposes the events of a bot instance
    /// </summary>
    public interface IAvatarEvents<TSender, TAvatar, TEventAvatarAddArgs, TEventAvatarClickArgs, TEventAvatarRemoveArgs>
        where TAvatar: MarshalIndefinite, IAvatar<TAvatar>
        where TEventAvatarAddArgs: MarshalIndefinite, IEventAvatarAddArgs<TAvatar>
        where TEventAvatarClickArgs : MarshalIndefinite, IEventAvatarClickArgs<TAvatar>
        where TEventAvatarRemoveArgs : MarshalIndefinite, IEventAvatarRemoveArgs<TAvatar>
    {
        /// <summary>
        /// Occurs when [avatar is added].
        /// </summary>
        event AvatarEventAddDelegate<TSender, TAvatar> AvatarEventAdd;
        /// <summary>
        /// Occurs when [avatar has changed].
        /// </summary>
        event AvatarEventChangeDelegate<TSender,TAvatar> AvatarEventChange;
        /// <summary>
        /// Occurs when [avatar has been removed].
        /// </summary>
        event AvatarEventRemoveDelegate<TSender,TAvatar> AvatarEventRemove;
    }
}