using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Templated;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void ObjectEventChangeDelegate(RemoteServices.RemotingBotEngine sender, ObjectChangeArgs e);

    public sealed class ObjectChangeArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        public Model OldModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectChangeArgs{TAvatar,TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="oldModel">The old model.</param>
        /// <param name="avatar">The avatar.</param>
        public ObjectChangeArgs(ICloneableT<Model> model, ICloneableT<Model> oldModel, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}