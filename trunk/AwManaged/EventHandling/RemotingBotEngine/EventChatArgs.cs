using System;
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void ChatEventDelegate(RemoteServices.RemotingBotEngine sender, EventChatArgs e);

    public sealed class EventChatArgs : MarshalByRefObject
    {
        public EventChatArgs(ICloneableT<Avatar> avatar, ChatType chatType, string message)
        {
            Avatar = avatar.Clone();
            ChatType = chatType;
            Message = message;
        }

        public string Message{get; private set;}
        public ChatType ChatType {get;private set;}
        public Avatar Avatar { get; private set;}
    }
}