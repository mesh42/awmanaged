using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventAvatarRemoveArgs<TAvatar> where TAvatar : IAvatar<TAvatar>
    {
        TAvatar Avatar { get; }
    }
}