using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void AvatarEventAddDelegate<TSender, TAvatar>(TSender sender, EventAvatarAddArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;
    /// <summary>
    /// 
    /// </summary>
    public sealed class EventAvatarAddArgs<TAvatar> : MarshalByRefObject, IEventAvatarAddArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        public TAvatar Avatar { get; private set; }

        public EventAvatarAddArgs(ICloneableT<TAvatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}