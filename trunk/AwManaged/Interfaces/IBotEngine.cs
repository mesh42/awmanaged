﻿using System;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.ExceptionHandling;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Interfaces
{
    /// <summary>
    /// Main Interface for building a bot engine (remoting support is added in the interface).
    /// The interface is build up as such, that a bot implementation can use different types of implementations 
    /// in order to support a factory pattern.
    /// </summary>
    public interface IBotEngine<TAvatar,TModel,TCamera,TZone, TMover, THudBase> : 
        IHandleExceptionManaged,
        ISceneNodeCommands<TModel,TAvatar,THudBase>,
        IChatCommands<TAvatar>,
        /*ISceneNodes<TModel,TCamera,TMover,TZone,THudBase,TAvatar>,*/
        IAvatarCommands<TAvatar>, IConfigurable
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TCamera : MarshalByRefObject, ICamera<TCamera>
        where TMover : MarshalByRefObject, IMover<TMover>
        where TZone : MarshalByRefObject, IZone<TZone, TModel, TCamera>
        where THudBase : MarshalByRefObject, IHudBase<THudBase, TAvatar>
    {
        /// <summary>
        /// Gets the scene nodes.
        /// </summary>
        /// <value>The scene nodes.</value>
        ISceneNodes<TModel, TCamera, TMover, TZone, THudBase, TAvatar> SceneNodes { get;}

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