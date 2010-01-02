using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void ObjectEventScanCompletedDelegate(RemoteServices.RemotingBotEngine sender, EventObjectScanCompletedEventArgs e);

    /// <summary>
    /// Raised when object scanning of a world has been completed.
    /// </summary>
    public sealed class EventObjectScanCompletedEventArgs : MarshalByRefObject
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