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

namespace AwManaged.Scene.Interfaces
{
    /// <summary>
    /// Scene node commands, allow for interacting with objects such as Camera's, Movers and Models (objects) in a managed fashion.
    /// </summary>
    public interface ISceneNodeCommands<TModel,TAvatar,THud>
        where TModel : MarshalIndefinite, IModel<TModel>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
        where THud : MarshalIndefinite, IHudBase<THud,TAvatar>
    {
        /// <summary>
        /// Displays a hud to the specified avatar.
        /// </summary>
        /// <param name="hud">The hud.</param>
        /// <param name="avatar">The avatar.</param>
        void HudDisplay(THud hud, TAvatar avatar);
        /// <summary>
        /// Deletes the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void DeleteObject(TModel o);
        /// <summary>
        /// Adds the V3 object.
        /// </summary>
        /// <param name="o">The o.</param>
        void AddObject(TModel o);
        /// <summary>
        /// Changes the object.
        /// </summary>
        /// <param name="model">The model.</param>
        void ChangeObject(TModel model);
        /// <summary>
        /// Changes the object with a specified delay.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="delay">The delay.</param>
        void ChangeObject(TModel model, int delay);
    }
}