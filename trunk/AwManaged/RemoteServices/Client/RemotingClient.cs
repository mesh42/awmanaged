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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Services;

namespace AwManaged.RemoteServices.Client
{
    public class RemotingClient<TRemotingBotEngine> : IConnectedServiceDevice<RemotingClientConnection>
    {
        private BinaryClientFormatterSinkProvider clientProvider;
        private BinaryServerFormatterSinkProvider serverProvider;
        private TcpChannel channelTcp;
        private HttpChannel channelHttp;
        private IpcChannel channelIpc;

        public string ProviderName
        {
            get { return "awmremoting"; }
        }


        IDictionary props = new Hashtable();

        public RemotingClient(string connectionString)
        {
            Connection = new RemotingClientConnection { ConnectionString = connectionString };
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
            {
                switch (item.Name.ToLower())
                {
                    case "provider":
                        break; // handled by the ConnectionStringHelper
                    case "protocol":
                        switch (item.Value.ToLower().ToLower())
                        {
                            case "ipc":
                                Connection.Protocol = RemotingProtocol.Ipc;
                                break;
                            case "tcp":
                                Connection.Protocol = RemotingProtocol.Tcp;
                                break;
                            case "http":
                                Connection.Protocol = RemotingProtocol.Http;
                                break;
                            default:
                                throw new Exception(string.Format("Protocol '{0}' is not supported.", item.Value));
                        }
                        break;
                    case "server":
                        break;

                    case "port":
                        try { Connection.Port = int.Parse(item.Value.Trim()); }
                        catch { ThrowIncorrectConnectionStringException(); }
                        break;
                    default:
                        ThrowIncorrectConnectionStringException();
                        break;
                }
            }
            EvaluateConnectionProperties();
        }

        private void EvaluateConnectionProperties()
        {
            if (Connection.Protocol == RemotingProtocol.Unspecified)
                throw new ArgumentException("Connection string does not contain mandatory protocol type.");
            if (Connection.Port <= 0)
                throw new ArgumentException("A port number has not been specified or should be a positive number.");
        }

        static void ThrowIncorrectConnectionStringException()
        {
            throw new ArgumentException("Connectionstring is in the incorrect format");
        }

        #region IService Members

        public bool Stop()
        {
            if (!IsRunning)
                throw new ArgumentException("Remoting Server could not be stopped as it not running.");
            switch (Connection.Protocol)
            {
                case RemotingProtocol.Ipc:
                    channelTcp.StopListening(null);
                    ChannelServices.UnregisterChannel(channelTcp);
                    break;
                case RemotingProtocol.Tcp:
                    channelTcp.StopListening(null);
                    ChannelServices.UnregisterChannel(channelTcp);
                    break;
                case RemotingProtocol.Http:
                    channelHttp.StopListening(null);
                    ChannelServices.UnregisterChannel(channelHttp);
                    break;
            }
            return true;
        }

        public bool Start()
        {
            if (IsRunning)
                throw new ArgumentException("Remoting client already started.");
            serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            props["port"] = Connection.Port;
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            switch (Connection.Protocol)
            {
                case RemotingProtocol.Ipc:
                    channelIpc = new IpcChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channelTcp, false);
                    break;
                case RemotingProtocol.Tcp:
                    channelTcp = new TcpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channelTcp, false);
                    break;
                case RemotingProtocol.Http:
                    channelHttp = new HttpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channelHttp, false);
                    break;
            }
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TRemotingBotEngine), "RemotingBotEngine", WellKnownObjectMode.Singleton);
            IsRunning = true; // todo make this smarter.
            return true;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        #endregion

        #region IConnectionProperties<RemotingServerConnection> Members


        public RemotingClientConnection Connection
        {
            get;
            private set;
        }

        #endregion

        #region IIdentifiable Members

        public string DisplayName
        {
            get; internal set;
        }

        public Guid Id
        {
            get; internal set;
        }

        public string TechnicalName
        {
            get; internal set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO: Cleanup.
        }

        #endregion
    }
}