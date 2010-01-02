using System;
using AwManaged.Core.Interfaces;
using AwManaged.RemoteServices;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    public delegate void BotEventLoggedInDelegate(RemoteServices.RemotingBotEngine sender, EventBotLoggedInArgs e);

    public class EventBotLoggedInArgs : MarshalByRefObject
    {
        public RemotingConnectionProperties  ConnectionProperties
        { 
            get; private set;
        }

        public EventBotLoggedInArgs(ICloneableT<RemotingConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }

    }
}