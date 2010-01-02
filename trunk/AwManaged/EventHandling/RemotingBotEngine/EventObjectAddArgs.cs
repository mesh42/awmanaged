using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Templated;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void ObjectEventAddDelegate(RemoteServices.RemotingBotEngine sender, EventObjectAddArgs e);

    public sealed class EventObjectAddArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventObjectClickArgs{TAvatar,TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="avatar">The avatar.</param>
        public EventObjectAddArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}