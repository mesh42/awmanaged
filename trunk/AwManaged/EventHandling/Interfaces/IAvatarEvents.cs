using System;
using AwManaged.EventHandling;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    /// <summary>
    /// Interface which exposes the events of a bot instance
    /// </summary>
    public interface IAvatarEvents<TSender, TAvatar, TEventAvatarAddArgs, TEventAvatarClickArgs, TEventAvatarRemoveArgs>
        where TAvatar: MarshalByRefObject, IAvatar<TAvatar>
        where TEventAvatarAddArgs: MarshalByRefObject, IEventAvatarAddArgs<TAvatar>
        where TEventAvatarClickArgs : MarshalByRefObject, IEventAvatarClickArgs<TAvatar>
        where TEventAvatarRemoveArgs : MarshalByRefObject, IEventAvatarRemoveArgs<TAvatar>
    {
        /// <summary>
        /// Occurs when [avatar is added].
        /// </summary>
        event AvatarEventAddDelegate<TSender, TAvatar> AvatarEventAdd;
        /// <summary>
        /// Occurs when [avatar has changed].
        /// </summary>
        event AvatarEventChangeDelegate<TSender,TAvatar> AvatarEventChange;
        /// <summary>
        /// Occurs when [avatar has been removed].
        /// </summary>
        event AvatarEventRemoveDelegate<TSender,TAvatar> AvatarEventRemove;
    }
}