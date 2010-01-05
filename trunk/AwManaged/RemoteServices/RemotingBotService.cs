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
using AwManaged.EventHandling.RemotingBotEngine;
using AwManaged.RemoteServices.Interfaces;

namespace AwManaged.RemoteServices
{
    public sealed class RemotingBotService : MarshalByRefObject, IRemoteBotService
    {
        #region Delegates and Events

        public event BotEventLoggedInDelegate BotEventLoggedIn;
        public event BotEventEntersWorldDelegate BotEventEntersWorld;
        public event AvatarEventAddDelegate AvatarEventAdd;
        public event AvatarEventChangeDelegate AvatarEventChange;
        public event AvatarEventRemoveDelegate AvatarEventRemove;
        public event ObjectEventClickDelegate ObjectEventClick;
        public event ObjectEventAddDelegate ObjectEventAdd;
        public event ObjectEventRemoveDelegate ObjectEventRemove;
        public event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        public event ChatEventDelegate ChatEvent;
        public event ObjectEventChangeDelegate ObjectEventChange;

        #endregion

        public RemotingBotService()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotingBotService"/> class.
        /// </summary>
        /// <param name="engine">The bot engine which is to be exposed over TCP.</param>
        /// <param name="port">The port.</param>
        public RemotingBotService(BotEngine engine, int port)
        {
            //ChannelServices.RegisterChannel(new HttpChannel(port),false);
            //ObjRef ref1 = RemotingServices.Marshal(engine, "http://localhost:9000/RemotingBotEngine");
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(BotEngine), "RemotingBotEngine", WellKnownObjectMode.SingleCall);
        }

        public RemotingBotEngine GetRemotingBotInstance()
        {
            return new RemotingBotEngine();
        }
    }
}