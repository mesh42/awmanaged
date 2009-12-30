using System;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void BotEventLoggedInDelegate<TSender, TConnectionProperties>(
        TSender sender, EventBotLoggedInArgs<TConnectionProperties> e)
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>;

    public class EventBotLoggedInArgs<TConnectionProperties> : IEventBotLoggedInArgs<TConnectionProperties>
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>
    {
        #region IEventBotLoggedInArgs<TConnectionProperties> Members

        public TConnectionProperties  ConnectionProperties
        { 
            get; private set;
        }

        #endregion


        public EventBotLoggedInArgs(ICloneableT<TConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }

    }
}
