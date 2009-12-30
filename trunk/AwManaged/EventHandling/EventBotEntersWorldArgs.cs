using System;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;

namespace AwManaged.EventHandling
{
    public delegate void BotEventEntersWorldDelegate<TSender, TConnectionProperties>(
        TSender sender, EventBotEntersWorldArgs<TConnectionProperties> e)
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>;

    public class EventBotEntersWorldArgs<TConnectionProperties> : IEventBotLoggedInArgs<TConnectionProperties>
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>
    {
        #region IEventBotEntersWorldArgs<TConnectionProperties> Members

        public TConnectionProperties ConnectionProperties
        {
            get;
            private set;
        }

        #endregion

        public EventBotEntersWorldArgs(ICloneableT<TConnectionProperties> connectionProperties)
        {
            ConnectionProperties = connectionProperties.Clone();
        }
    }
}
