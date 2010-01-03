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
using AwManaged.Core.Interfaces;
using AwManaged.RemoteServices;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
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