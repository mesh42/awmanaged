using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void AvatarEventChangeDelegate(RemoteServices.RemotingBotEngine sender, EventAvatarChangeArgs e);

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