using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void AvatarEventChangeDelegate<TSender,TAvatar>(BotEngine sender, EventAvatarAddArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;

    public class EventAvatarChangeArgs<TAvatar> : MarshalByRefObject, IEventAvatarChangeArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        #region IEventAvatarChangeArgs<TAvatar> Members

        public TAvatar AvatarPreviousState
        {
            get; private set;
        }

        public TAvatar Avatar
        {
            get; private set;
        }

        public EventAvatarChangeArgs(ICloneableT<TAvatar> avatar, ICloneableT<TAvatar> avatarPreviousState)
        {
            Avatar = avatar.Clone();
            AvatarPreviousState = avatarPreviousState.Clone();
        }

        #endregion
    }
}
