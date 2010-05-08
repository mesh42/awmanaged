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
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void BotEventLoggedInDelegate(AwManaged.BotEngine sender, EventBotLoggedInArgs e);

    public class EventBotLoggedInArgs : MarshalIndefinite
    {
        private readonly int _node;

        public int Node
        {
            get { return _node; }
        }

        public UniverseConnectionProperties  ConnectionProperties
        { 
            get; private set;
        }

        public EventBotLoggedInArgs(ICloneableT<UniverseConnectionProperties> connectionProperties, int node)
        {
            _node = node;
            ConnectionProperties = connectionProperties.Clone();
        }
    }
}