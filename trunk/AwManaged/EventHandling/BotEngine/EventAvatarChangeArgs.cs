using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void AvatarEventChangeDelegate(AwManaged.BotEngine sender, EventAvatarChangeArgs e);

    public class EventAvatarChangeArgs : MarshalByRefObject
    {
        public Avatar AvatarPreviousState
        {
            get; private set;
        }

        public Avatar Avatar
        {
            get; private set;
        }

        public EventAvatarChangeArgs(ICloneableT<Avatar> avatar, ICloneableT<Avatar> avatarPreviousState)
        {
            Avatar = avatar.Clone();
            AvatarPreviousState = avatarPreviousState.Clone();
        }
    }
}