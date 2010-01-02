using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void ObjectEventClickDelegate(RemoteServices.RemotingBotEngine sender, EventObjectClickArgs e);

    public sealed class EventObjectClickArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }

        public EventObjectClickArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}