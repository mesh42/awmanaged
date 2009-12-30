using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectChangeArgs<TModel,TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user changed.
        /// </summary>
        /// <value>The object.</value>
        TModel Model { get; }
        /// <summary>
        /// The old object before the user changed it.
        /// </summary>
        /// <value>The object.</value>
        TModel OldModel { get; }
        /// <summary>
        /// The user who changed the object.
        /// </summary>
        /// <value>The avatar.</value>
        TAvatar Avatar { get; }
    }
}