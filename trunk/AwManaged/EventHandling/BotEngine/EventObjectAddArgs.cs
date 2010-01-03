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

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void ObjectEventAddDelegate(AwManaged.BotEngine sender, EventObjectAddArgs e);

    public sealed class EventObjectAddArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        public EventObjectAddArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}