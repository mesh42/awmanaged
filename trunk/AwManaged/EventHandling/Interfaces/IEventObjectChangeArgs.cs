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
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectChangeArgs<TModel,TAvatar>
        where TModel : MarshalIndefinite, IModel<TModel>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user changed.
        /// </summary>
        /// <value>The object.</value>
        TModel Model { get; }
        /// <summary>
        /// The old object before the user changed it.
        /// </summary>
        /// <value>The object.</value>
        TModel OldModel { get; }
        /// <summary>
        /// The user who changed the object.
        /// </summary>
        /// <value>The avatar.</value>
        TAvatar Avatar { get; }
    }
}