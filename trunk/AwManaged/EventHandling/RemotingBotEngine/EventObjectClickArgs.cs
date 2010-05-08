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
    public delegate void ObjectEventClickDelegate(RemoteServices.RemotingBotEngine sender, EventObjectClickArgs e);

    public sealed class EventObjectClickArgs : MarshalIndefinite
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }

        public EventObjectClickArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}