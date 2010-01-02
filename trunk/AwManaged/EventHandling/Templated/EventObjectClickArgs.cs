using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void ObjectEventClickDelegate<TSender, TAvatar, TModel>(TSender sender, EventObjectClickArgs<TAvatar,TModel> e)
        where TAvatar: MarshalByRefObject, IAvatar<TAvatar>
        where TModel: MarshalByRefObject, IModel<TModel>;

    public sealed class EventObjectClickArgs<TAvatar,TModel> : MarshalByRefObject, IEventObjectClickArgs<TModel,TAvatar>
        where TAvatar: MarshalByRefObject, IAvatar<TAvatar>
        where TModel: MarshalByRefObject, IModel<TModel>
    {
        public TModel Model { get; private set; }
        public TAvatar Avatar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectClickArgs&lt;TAvatar, TModel&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectClickArgs(ICloneableT<TModel> model, ICloneableT<TAvatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}