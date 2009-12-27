using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    /// <summary>
    /// Event Avatar Clicked Argments.
    /// </summary>
    /// <typeparam name="TAvatar">The type of the avatar.</typeparam>
    public interface IEventAvatarClickArgs<TAvatar> where TAvatar : IAvatar<TAvatar>
    {
        TAvatar Avatar { get; }
    }
}