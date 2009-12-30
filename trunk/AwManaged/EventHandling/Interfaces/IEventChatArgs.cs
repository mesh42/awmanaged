using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventChatArgs<TAvatar> where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
    {
        ChatType ChatType { get; }
        TAvatar Avatar { get; }
        string Message { get; }
    }
}