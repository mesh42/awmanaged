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
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void ObjectEventRemoveDelegate(RemoteServices.RemotingBotEngine sender, EventObjectRemoveArgs e);

    public sealed class EventObjectRemoveArgs : MarshalByRefObject
    {
        public Model Model { get; private set;}
        public Avatar Avatar { get; private set;}

        public EventObjectRemoveArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar )
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}