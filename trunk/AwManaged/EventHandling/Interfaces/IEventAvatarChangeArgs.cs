using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventAvatarChangeArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        TAvatar AvatarPreviousState { get; }
        TAvatar Avatar { get; }
    }
}
