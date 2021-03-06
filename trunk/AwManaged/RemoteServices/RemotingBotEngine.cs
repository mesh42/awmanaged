﻿/* **********************************************************************************
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
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace AwManaged.RemoteServices
{
    public class RemotingBotEngine : MarshalIndefinite
    {
        public void CallEventTest()
        {
        }

        public void CallBack(Delegate d)
        {

        }

        public static RemotingBotEngine RegisterClientInstance()
        {
            var clientProvider = new BinaryClientFormatterSinkProvider();
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 0;
            string s = Guid.NewGuid().ToString();
            props["name"] = s;
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            TcpChannel chan = new TcpChannel(props, null, serverProvider);
            ChannelServices.RegisterChannel(chan);
            Type typeofRI = typeof(RemotingBotEngine);
            return (RemotingBotEngine)Activator.GetObject(typeofRI, "tcp://localhost:9000/RemotingBotEngine");
        }
    }
}
