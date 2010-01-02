using System;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Templated
{
    public delegate void ChatEventDelegate<TSender, TAvatar>(TSender sender, EventChatArgs<TAvatar> e)
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>;

    public sealed class EventChatArgs<TAvatar> : IEventChatArgs<TAvatar>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        public EventChatArgs(ICloneableT<TAvatar> avatar, ChatType chatType, string message)
        {
            Avatar = avatar.Clone();
            ChatType = chatType;
            Message = message;
        }

        public string Message{get; private set;}
        public ChatType ChatType {get;private set;}
        public TAvatar Avatar { get; private set;}
    }
}