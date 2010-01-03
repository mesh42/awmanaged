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
    public delegate void ObjectEventChangeDelegate<TSender, TAvatar, TModel>(
        TSender sender, EventObjectChangeArgs<TAvatar, TModel> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>;

    public sealed class EventObjectChangeArgs<TAvatar,TModel> : IEventObjectChangeArgs<TModel, TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
    {
        public TModel Model { get; private set; }
        public TAvatar Avatar { get; private set; }
        public TModel OldModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectChangeArgs&lt;TAvatar, TModel&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="oldModel">The old model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectChangeArgs(ICloneableT<TModel> model, ICloneableT<TModel> oldModel, ICloneableT<TAvatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}