using System;
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;

namespace AwManaged.EventHandling.BotEngine
{
    public delegate void BotEventLoggedInDelegate(AwManaged.BotEngine sender, EventBotLoggedInArgs e);

    public class EventBotLoggedInArgs : MarshalByRefObject
    {
        
        public UniverseConnectionProperties  ConnectionProperties
        { 
            get; private set;
        }

        public EventBotLoggedInArgs(ICloneableT<UniverseConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }

    }
}