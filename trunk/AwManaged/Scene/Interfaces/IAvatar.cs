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
using AwManaged.Math;
using AwManaged.Scene;

namespace AwManaged.Scene.Interfaces
{
    public interface IAvatar<T> : ISceneNode<T>
        where T : MarshalIndefinite
    {
        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>The session.</value>
        int Session { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        Vector3 Position { get; set; }
        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        Vector3 Rotation { get; set; }
        /// <summary>
        /// Gets or sets the gesture.
        /// </summary>
        /// <value>The gesture.</value>
        int Gesture { get; set; }
        /// <summary>
        /// Gets or sets the citizen.
        /// </summary>
        /// <value>The citizen.</value>
        int Citizen { get; set; }
        /// <summary>
        /// Gets or sets the privilege.
        /// </summary>
        /// <value>The privilege.</value>
        int Privilege { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        int State { get; set; }
        /// <summary>
        /// Occurs when [on change position].
        /// </summary>
        event Avatar.OnChangePositionDelegate OnChangePosition;
        /// <summary>
        /// Changed the position.
        /// </summary>
        void ChangedPosition();
    }
}