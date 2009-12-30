using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;

namespace AwManaged.EventHandling
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