using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventChatArgs
    {
        ChatType ChatType { get; }
        IAvatar Avatar { get; }
        string Message { get; }
    }
}