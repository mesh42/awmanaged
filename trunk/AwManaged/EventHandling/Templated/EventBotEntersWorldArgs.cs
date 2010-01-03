/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;

namespace AwManaged.EventHandling.Templated
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