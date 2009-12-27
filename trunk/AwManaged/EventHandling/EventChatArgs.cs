using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public sealed class EventChatArgs : IEventChatArgs<Avatar>
    {
        public EventChatArgs(ICloneableT<Avatar> avatar, ChatType chatType, string message)
        {
            this.Avatar = avatar.Clone();
            this.ChatType = chatType;
            this.Message = message;
        }

        public string Message{get; private set;}
        public ChatType ChatType {get;private set;}
        public Avatar Avatar { get; private set;}
    }
}