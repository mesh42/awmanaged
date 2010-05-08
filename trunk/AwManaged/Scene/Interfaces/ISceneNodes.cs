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
using AwManaged.Core.Patterns;

namespace AwManaged.Scene.Interfaces
{
    /// <summary>
    /// Managed scene nodes.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCamera">The type of the camera.</typeparam>
    /// <typeparam name="TMover">The type of the mover.</typeparam>
    /// <typeparam name="TZone">The type of the zone.</typeparam>
    /// <typeparam name="THudBase">The type of the hud base.</typeparam>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    /// <typeparam name="TParticle">The type of the particle.</typeparam>
    /// <typeparam name="TParticleFlags">The type of the particle flags.</typeparam>
    public interface ISceneNodes<TModel, TCamera, TMover, TZone, THudBase, TAvatar, TParticle, TParticleFlags>
        where TModel : MarshalIndefinite, IModel<TModel>
        where TCamera : MarshalIndefinite, ICamera<TCamera>
        where TMover : MarshalIndefinite, IMover<TMover>
        where THudBase : MarshalIndefinite, IHudBase<THudBase, TAvatar>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
        where TZone : MarshalIndefinite, IZone<TZone, TModel, TCamera>
        where TParticle : MarshalIndefinite, IParticle<TParticle, TParticleFlags>
        where TParticleFlags : MarshalIndefinite, IParticleFlags<TParticleFlags>
    {
        /// <summary>
        /// Gets the avatars.
        /// </summary>
        /// <value>The avatars.</value>
        ProtectedList<TAvatar> Avatars { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the model cache.
        /// </summary>
        /// <value>The model.</value>
        ProtectedList<TModel> Models { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the camera cache.
        /// </summary>
        /// <value>The cameras.</value>
        ProtectedList<TCamera> Cameras { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the movers cache.
        /// </summary>
        /// <value>The movers.</value>
        ProtectedList<TMover> Movers { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the zones cache.
        /// </summary>
        /// <value>The zones.</value>
        ProtectedList<TZone> Zones { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the huds cache.
        /// </summary>
        /// <value>The huds.</value>
        ProtectedList<THudBase> Huds { get; }
        /// <summary>
        /// Gets a shallow memberwise clone of the particles cache.
        /// </summary>
        /// <value>The huds.</value>
        ProtectedList<TParticle> Particles { get; }


    }
}