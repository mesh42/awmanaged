using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectClickArgs<TModel, TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        /// <summary>
        /// The object the user clicked on.
        /// </summary>
        TModel Model { get; }
        /// <summary>
        /// The user who clicked the object.
        /// </summary>
        TAvatar Avatar { get; }
    }
}