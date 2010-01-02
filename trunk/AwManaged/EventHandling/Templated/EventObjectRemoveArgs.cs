using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void ObjectEventRemoveDelegate<TSender, TAvatar, TModel>(TSender sender, EventObjectRemoveArgs<TAvatar,TModel> e)
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;

    public sealed class EventObjectRemoveArgs<TAvatar, TModel> : MarshalByRefObject, IEventObjectRemoveArgs<TModel,TAvatar>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        #region IEventObjectRemoveArgs<Model,Avatar> Members

        public TModel Model { get; private set;}
        public TAvatar Avatar { get; private set;}

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectRemoveArgs&lt;TModel, TAvatar&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectRemoveArgs(ICloneableT<TModel> model, ICloneableT<TAvatar> avatar )
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}