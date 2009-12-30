using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IChatEvents<TSender, TAvatar, TEventChatArgs>
        where TAvatar : MarshalByRefObject, IAvatar<TAvatar>
        where TEventChatArgs : MarshalByRefObject, IEventChatArgs<TAvatar>
    {
        /// <summary>
        /// Occurs when [a chat event has been received].
        /// </summary>
        event ChatEventDelegate<TSender,TAvatar> ChatEvent;
    }
}
