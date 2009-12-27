using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventChatArgs<TAvatar> where TAvatar : IAvatar<TAvatar>
    {
        ChatType ChatType { get; }
        TAvatar Avatar { get; }
        string Message { get; }
    }
}