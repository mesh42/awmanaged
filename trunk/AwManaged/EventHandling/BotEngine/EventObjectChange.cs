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
    public delegate void ObjectEventChangeDelegate(AwManaged.BotEngine sender, EventObjectChangeArgs e);

    public sealed class EventObjectChangeArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        public Model OldModel { get; private set; }

        public EventObjectChangeArgs(ICloneableT<Model> model , ICloneableT<Model> oldModel, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}