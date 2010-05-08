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
using AwManaged.EventHandling.Templated;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void ObjectEventAddDelegate(RemoteServices.RemotingBotEngine sender, EventObjectAddArgs e);

    public sealed class EventObjectAddArgs : MarshalIndefinite
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectClickArgs{TAvatar,TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectAddArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}