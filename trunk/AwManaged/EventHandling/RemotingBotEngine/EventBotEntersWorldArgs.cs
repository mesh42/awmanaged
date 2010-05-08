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
using SharedMemory;using System;
using AwManaged.Core.Interfaces;
using AwManaged.RemoteServices;

namespace AwManaged.EventHandling.RemotingBotEngine
{
    [Serializable]
    public delegate void BotEventEntersWorldDelegate(RemoteServices.RemotingBotEngine sender, EventBotEntersWorldArgs e);

    public class EventBotEntersWorldArgs : MarshalIndefinite
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