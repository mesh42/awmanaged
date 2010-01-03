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
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void AvatarEventRemoveDelegate<TSender,TAvatar>(TSender sender, EventAvatarRemoveArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;

    /// <summary>
    /// This event gets fired when an avatar is removed from the world list.
    /// </summary>
    public sealed class EventAvatarRemoveArgs<TAvatar> : MarshalByRefObject, IEventAvatarRemoveArgs<TAvatar>
        where TAvatar: MarshalByRefObject, IAvatar<TAvatar>
    {
        public TAvatar Avatar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAvatarRemoveArgs&lt;TAvatar&gt;"/> class.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        public EventAvatarRemoveArgs(ICloneableT<TAvatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}