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
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.Core.ServicesManaging.Interfaces;
using AwManaged.EventHandling.BotEngine.Interfaces;
using AwManaged.ExceptionHandling;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// Main Interface for building a bot engine (remoting support is added in the interface).
    /// The interface is build up as such, that a bot implementation can use different types of implementations
    /// in order to support a factory pattern.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCamera">The type of the camera.</typeparam>
    /// <typeparam name="TZone">The type of the zone.</typeparam>
    /// <typeparam name="TMover">The type of the mover.</typeparam>
    /// <typeparam name="THudBase">The type of the hud base.</typeparam>
    /// <typeparam name="TParticle">The type of the particle.</typeparam>
    /// <typeparam name="TParticleFlags">The type of the particle flags.</typeparam>
    /// <typeparam name="TConnectionInterface">The type of the connection interface.</typeparam>
    /// <typeparam name="TLocalBotServicesManager">The type of the local bot services manager.</typeparam>
    public interface IBotEngine<TAvatar, TModel, TCamera, TZone, TMover, THudBase, TParticle, TParticleFlags,
                                TConnectionInterface, TLocalBotServicesManager> :  
        IIdentifiable,
        IBotEngineEvents,
        IHandleExceptionManaged,
        ISceneNodeCommands<TModel, TAvatar, THudBase>,
        IChatCommands<TAvatar>,
        IAvatarCommands<TAvatar>, IConfigurable
        where TModel : MarshalIndefinite, IModel<TModel>
        where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
        where TCamera : MarshalIndefinite, ICamera<TCamera>
        where TMover : MarshalIndefinite, IMover<TMover>
        where TZone : MarshalIndefinite, IZone<TZone, TModel, TCamera>
        where THudBase : MarshalIndefinite, IHudBase<THudBase, TAvatar>
        where TParticle : MarshalIndefinite, IParticle<TParticle, TParticleFlags>
        where TParticleFlags : MarshalIndefinite, IParticleFlags<TParticleFlags>
        where TConnectionInterface : IConnection<TConnectionInterface>
        where TLocalBotServicesManager : IServicesManager
    {
        IServicesManager ServicesManager { get; }
        TLocalBotServicesManager LocalBotPluginServicesManager { get; }

        /// <summary>
        /// Gets the scene nodes.
        /// </summary>
        /// <value>The scene nodes.</value>
        ISceneNodes<TModel, TCamera, TMover, TZone, THudBase, TAvatar, TParticle, TParticleFlags> SceneNodes { get;}
        LoginConfiguration LoginConfiguration { get; }
        /// <summary>
        /// Indicates if the privileged user account is logged on in global mode (Caretaker capable).
        /// </summary>
        bool IsEnterGlobal { get; }
        /// <summary>
        /// Indicates to echo all chat messages that are received. Helpfull for color bots, when the world attributes
        /// are set to not echo the chat.
        /// </summary>
        bool IsEchoChat { get; set; }
        /// <summary>
        /// Scan's objects in either global or non global mode. (auto sensing mode)
        /// </summary>
        void ScanObjects();
        /// <summary>
        /// Starts this bot instance by logging in to the world under
        /// Prefered Care Taker privileges (enter global mode). If the privileged user account
        /// does not have caretaker capabilities for the world, we will automatically
        /// downgrade the privileges so the user can log on non-global.
        /// </summary>
        void Start();
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        void Dispose();
    }
}