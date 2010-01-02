using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void AvatarEventAddDelegate(RemoteServices.RemotingBotEngine sender, EventAvatarAddArgs e);

    public sealed class EventAvatarAddArgs : MarshalByRefObject
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarAddArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}