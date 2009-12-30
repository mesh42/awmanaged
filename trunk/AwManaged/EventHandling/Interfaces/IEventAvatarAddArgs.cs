using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    /// <summary>
    /// Avatar Event Add Arguments.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    public interface IEventAvatarAddArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        TAvatar Avatar { get; }
    }
}