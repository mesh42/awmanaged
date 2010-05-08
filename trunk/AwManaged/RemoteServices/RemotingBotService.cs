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
using AwManaged.Core.Services;
using AwManaged.EventHandling.RemotingBotEngine;

namespace AwManaged.RemoteServices
{
    public sealed class RemotingBotService : BaseConnectedServiceDevice<RemotingBotServerConnection>, INeedBotEngineInstance<BotEngine>
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

        public RemotingBotService(string connection) : base (connection)
        {
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
            {
                switch (item.Name.ToLower())
                {
                    case "provider":
                        break; // handled by the ConnectionStringHelper
                    case "port":
                        try { Connection.Port = int.Parse(item.Value.Trim()); }
                        catch { ThrowIncorrectConnectionStringException(); }
                        break;
                    case "protocol":
                        switch (item.Value.Trim().ToLower())
                        {
                            case "tcp":
                                Connection.Protocol = RemotingProtocol.Tcp;
                                break;
                            case "http":
                                Connection.Protocol = RemotingProtocol.Http;
                                break;
                            case "ipc":
                                Connection.Protocol = RemotingProtocol.Ipc;
                                break;
                            default:
                                ThrowIncorrectConnectionStringException();
                                break;
                        }
                        break;
                    default:
                        ThrowIncorrectConnectionStringException();
                        break;
                }
            }
        }

        public RemotingBotEngine GetRemotingBotInstance()
        {
            return new RemotingBotEngine();
        }

        public override string IdentifyableDisplayName
        {
            get { return "Remoting Bot Service."; }
        }

        public override Guid IdentifyableId
        {
            get { return new Guid("{7C8A778A-EE22-4e39-B568-FF8295974513}"); }
        }

        //public override string TechnicalName
        //{
        //    get { return ProviderName; }
        //}

        public override void Dispose()
        {
            Stop();
        }

        public override string ProviderName
        {
            get { return "awmremoting"; }
        }

        internal override void EvaluateConnectionProperties()
        {
            if (Connection.Protocol == RemotingProtocol.Unspecified)
                throw new Exception("Remoting protocol is not specified.");
            if (Connection.Port == 0)
                throw new Exception("Remoting port is not specified.");
        }

        public override bool Start()
        {
            return base.Start();

        }

        public override bool Stop()
        {
            // todo: signal all connected clients of shutdown.
            return base.Stop();
        }

        #region INeedBotEngineInstance<BotEngine> Members

        public BotEngine BotEngine
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}