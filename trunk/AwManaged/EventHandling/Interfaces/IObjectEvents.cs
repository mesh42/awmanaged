using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Templated;
using AwManaged.Scene;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    interface IObjectEvents<TSender, TAvatar, TModel, TSceneNodes> 
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TSceneNodes : MarshalByRefObject, ICloneableT<TSceneNodes>
    {
        event ObjectEventClickDelegate<TSender, TAvatar,TModel> ObjectEventClick;
        /// <summary>
        /// Occurs when [an object is added to the world].
        /// </summary>
        event ObjectEventAddDelegate<TSender, TAvatar,TModel> ObjectEventAdd;
        /// <summary>
        /// Occurs when [an object is removed from the world].
        /// </summary>
        event ObjectEventRemoveDelegate<TSender, TAvatar, TModel> ObjectEventRemove;
        /// <summary>
        /// Occurs when [object event scan has been completed].
        /// </summary>
        event ObjectEventScanCompletedDelegate<TSender, TSceneNodes> ObjectEventScanCompleted;
        /// <summary>
        /// Occurs when [an object in the world has changed].
        /// </summary>
        event ObjectEventChangeDelegate<TSender, TAvatar, TModel> ObjectEventChange;
    }
}
