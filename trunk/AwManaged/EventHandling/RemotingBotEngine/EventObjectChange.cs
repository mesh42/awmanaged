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
using AwManaged.EventHandling.Templated;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void ObjectEventChangeDelegate(RemoteServices.RemotingBotEngine sender, ObjectChangeArgs e);

    public sealed class ObjectChangeArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        public Model OldModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectChangeArgs{TAvatar,TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="oldModel">The old model.</param>
        /// <param name="avatar">The avatar.</param>
        public ObjectChangeArgs(ICloneableT<Model> model, ICloneableT<Model> oldModel, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}