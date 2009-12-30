using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    /// <summary>
    /// Event Avatar Clicked Argments.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    public interface IEventAvatarClickArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        TAvatar Avatar { get; }
    }
}