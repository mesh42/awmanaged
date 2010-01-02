using System;
using AwManaged.Core.Interfaces;
using AwManaged.RemoteServices;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void BotEventEntersWorldDelegate(RemoteServices.RemotingBotEngine sender, EventBotEntersWorldArgs e);

    public class EventBotEntersWorldArgs : MarshalByRefObject
    {
        public RemotingConnectionProperties ConnectionProperties
        {
            get;private set;
        }

        public EventBotEntersWorldArgs(ICloneableT<RemotingConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }
    }
}