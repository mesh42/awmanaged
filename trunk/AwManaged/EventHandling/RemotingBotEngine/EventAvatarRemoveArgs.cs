using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void AvatarEventRemoveDelegate(RemoteServices.RemotingBotEngine sender, EventAvatarRemoveArgs e);

    /// <summary>
    /// This event gets fired when an avatar is removed from the world list.
    /// </summary>
    public sealed class EventAvatarRemoveArgs : MarshalByRefObject
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarRemoveArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}