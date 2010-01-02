using System;
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;

namespace AwManaged.EventHandling.BotEngine
{
    public delegate void BotEventEntersWorldDelegate(AwManaged.BotEngine sender, EventBotEntersWorldArgs e);

    public class EventBotEntersWorldArgs : MarshalByRefObject
    {
        public UniverseConnectionProperties ConnectionProperties{get;private set;}

        public EventBotEntersWorldArgs(ICloneableT<UniverseConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }
    }
}