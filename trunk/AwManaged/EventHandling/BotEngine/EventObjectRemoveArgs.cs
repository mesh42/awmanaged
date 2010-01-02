using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.BotEngine
{
    public delegate void ObjectEventRemoveDelegate(AwManaged.BotEngine sender, EventObjectRemoveArgs e);

    public sealed class EventObjectRemoveArgs : MarshalByRefObject
    {
        public Model Model { get; private set;}
        public Avatar Avatar { get; private set;}

        public EventObjectRemoveArgs(ICloneableT<Model> model, ICloneableT<Avatar> avatar )
        {
            Model = model.Clone();
            Avatar = avatar.Clone();
        }
    }
}