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
using System.ServiceModel;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene.Interfaces
{
    /// <summary>
    /// Avatar commands.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar, which needs to implement IAvatar</typeparam>
    [ServiceContract]
    public interface IAvatarCommands<TAvatar> where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
    {
        /// <summary>
        /// Sets the avatar's position and yaw.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="position">The position.</param>
        /// <param name="yaw">The yaw.</param>
        [OperationContract]
        void Teleport(TAvatar avatar, Vector3 position, float yaw);
        /// <summary>
        /// Sets the avatar's position.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="yaw">The yaw.</param>
        [OperationContract]
        void Teleport(TAvatar avatar, float x, float y, float z, float yaw);
        /// <summary>
        /// Gets the ciziten name by number. This routine is very slow
        /// The Universe server does niet allow for rapid queries. Ther should be about 3 seconds between
        /// each query. Probably some hardcoded time interval by Active Worlds, to prevent
        /// high server load.
        /// </summary>
        /// <param name="citizen">The citizen.</param>
        /// <returns></returns>
        [OperationContract]
        string GetCizitenNameByNumber(int citizen);
    }
}