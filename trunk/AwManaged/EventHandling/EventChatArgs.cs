using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling
{
    public class EventChatArgs : IEventChatArgs
    {
        private readonly IAvatar avatar;
        private readonly ChatType chatType;
        private readonly string message;

        public EventChatArgs(IAvatar avatar, ChatType chatType, string message)
        {
            this.avatar = avatar;
            this.chatType = chatType;
            this.message = message;
        }

        /// <summary>
        /// Gets the chat message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Gets the type of the chat message.
        /// </summary>
        /// <value>The type of the chat.</value>
        public ChatType ChatType
        {
            get { return chatType; }
        }

        /// <summary>
        /// Gets the avatar from which this message was sent.
        /// </summary>
        /// <value>The avatar.</value>
        public IAvatar Avatar
        {
            get { return avatar; }
        }
    }
}