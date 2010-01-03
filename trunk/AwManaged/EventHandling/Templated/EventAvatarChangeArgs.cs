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
    public delegate void AvatarEventChangeDelegate<TSender,TAvatar>(TSender sender, EventAvatarAddArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;

    public class EventAvatarChangeArgs<TAvatar> : MarshalByRefObject, IEventAvatarChangeArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        #region IEventAvatarChangeArgs<TAvatar> Members

        public TAvatar AvatarPreviousState
        {
            get; private set;
        }

        public TAvatar Avatar
        {
            get; private set;
        }

        #endregion

        public EventAvatarChangeArgs(ICloneableT<TAvatar> avatar, ICloneableT<TAvatar> avatarPreviousState)
        {
            Avatar = avatar.Clone();
            AvatarPreviousState = avatarPreviousState.Clone();
        }
    }
}