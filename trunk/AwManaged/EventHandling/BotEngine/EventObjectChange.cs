using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.BotEngine
{
    public delegate void ObjectEventChangeDelegate(AwManaged.BotEngine sender, EventObjectChangeArgs e);

    public sealed class EventObjectChangeArgs : MarshalByRefObject
    {
        public Model Model { get; private set; }
        public Avatar Avatar { get; private set; }
        public Model OldModel { get; private set; }

        public EventObjectChangeArgs(ICloneableT<Model> model, ICloneableT<Model> oldModel, ICloneableT<Avatar> avatar)
        {
            Model = model.Clone();
            OldModel = oldModel.Clone();
            Avatar = avatar.Clone();
        }
    }
}