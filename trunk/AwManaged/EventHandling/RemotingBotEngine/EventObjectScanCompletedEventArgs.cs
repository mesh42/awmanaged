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
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void ObjectEventScanCompletedDelegate(RemoteServices.RemotingBotEngine sender, EventObjectScanCompletedEventArgs e);

    /// <summary>
    /// Raised when object scanning of a world has been completed.
    /// </summary>
    public sealed class EventObjectScanCompletedEventArgs : MarshalIndefinite
    {
        public EventObjectScanCompletedEventArgs(ICloneableT<SceneNodes> sceneNodes)
        {
            SceneNodes = sceneNodes.Clone(); // NOTE: this could be rather memory and time consuming.
        }

        public SceneNodes SceneNodes
        {
            get; internal set; 
        }
    }
}