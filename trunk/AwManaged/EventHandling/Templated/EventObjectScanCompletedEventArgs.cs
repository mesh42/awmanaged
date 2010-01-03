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

namespace AwManaged.EventHandling.Templated
{
    public delegate void ObjectEventScanCompletedDelegate<TSender,TSceneNodes>(TSender sender, EventObjectScanCompletedEventArgs<TSceneNodes> e)
        where TSceneNodes : MarshalByRefObject, ICloneableT<TSceneNodes>;

    /// <summary>
    /// Raised when object scanning of a world has been completed.
    /// </summary>
    public sealed class EventObjectScanCompletedEventArgs<TSceneNodes> : MarshalByRefObject, IEventObjectScanEventArgs<TSceneNodes>
        where TSceneNodes : ICloneableT<TSceneNodes>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectScanCompletedEventArgs&lt;TSceneNodes&gt;"/> class.
        /// </summary>
        /// <param name="sceneNodes">The scene nodes.</param>
        public EventObjectScanCompletedEventArgs(ICloneableT<TSceneNodes> sceneNodes)
        {
            SceneNodes = sceneNodes.Clone(); // NOTE: this could be rather memory and time consuming.
        }

        #region IEventObjectScanEventArgs<SceneNodes> Members

        public TSceneNodes SceneNodes
        {
            get; internal set; 
        }

        #endregion
    }
}