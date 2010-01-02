using System;
using AwManaged.Configuration.Interfaces;
using AwManaged.EventHandling.Templated;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IBotEvents<TSender, TConnectionProperties>
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>
    {
        /// <summary>
        /// Occurs when [bot enters world].
        /// </summary>
        event BotEventEntersWorldDelegate<TSender,TConnectionProperties> BotEventEntersWorld;
        /// <summary>
        /// Occurs when [bot logged in].
        /// </summary>
        event BotEventLoggedInDelegate<TSender, TConnectionProperties> BotEventLoggedIn;
        /// <summary>
        /// Occurs when [avatar clicks on an object].
        /// </summary>

    }
}
