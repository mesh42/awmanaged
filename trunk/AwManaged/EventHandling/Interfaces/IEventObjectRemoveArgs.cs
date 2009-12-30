using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectRemoveArgs<TModel, TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user removed. (This is a cloned object of the original, as the actual object does not exist anymore with the AW SDK implementation).
        /// </summary>
        /// <value>The object.</value>
        TModel Model { get; }
        /// <summary>
        /// The user who removed the object.
        /// </summary>
        /// <value>The avatar.</value>
        TAvatar Avatar { get; }
    }
}