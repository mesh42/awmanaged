using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void ObjectEventChangeDelegate<TSender, TAvatar, TModel>(
        TSender sender, EventObjectChangeArgs<TAvatar, TModel> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>;

    public sealed class EventObjectChangeArgs<TAvatar,TModel> : IEventObjectChangeArgs<TModel, TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
    {
        public TModel Model { get; private set; }
        public TAvatar Avatar { get; private set; }
        public TModel OldModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectChangeArgs&lt;TAvatar, TModel&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="oldModel">The old model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectChangeArgs(ICloneableT<TModel> model, ICloneableT<TModel> oldModel, ICloneableT<TAvatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}