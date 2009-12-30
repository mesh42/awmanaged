using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void AvatarEventRemoveDelegate<TSender,TAvatar>(TSender sender, EventAvatarRemoveArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;
    /// <summary>
    /// This event gets fired when an avatar is removed from the world list.
    /// </summary>
    public sealed class EventAvatarRemoveArgs<TAvatar> : MarshalByRefObject, IEventAvatarRemoveArgs<TAvatar>
        where TAvatar: MarshalByRefObject, IAvatar<TAvatar>
    {
        public TAvatar Avatar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAvatarRemoveArgs&lt;TAvatar&gt;"/> class.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        public EventAvatarRemoveArgs(ICloneableT<TAvatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}