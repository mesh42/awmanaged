using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void ObjectEventAddDelegate<TSender,TAvatar,TModel>(TSender sender, EventObjectAddArgs<TAvatar,TModel> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>;

    public sealed class EventObjectAddArgs<TAvatar, TModel> : MarshalByRefObject, IEventObjectAddArgs<TModel, TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
    {
        public TModel Model { get; private set; }
        public TAvatar Avatar { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectClickArgs&lt;TAvatar, TModel&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectAddArgs(ICloneableT<TModel> model, ICloneableT<TAvatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}