using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Services;
using AwManaged.Security.RemoteBotEngine;
using AwManaged.Security.RemoteBotEngine.Interfaces;

namespace AwManaged.RemoteServices.Server
{
    public class RemotingServer<TRemotingBotEngine> : IConnectedServiceDevice<RemotingServerConnection>
    {
        private BinaryClientFormatterSinkProvider clientProvider;
        private BinaryServerFormatterSinkProvider serverProvider;
        private TcpChannel channelTcp;
        private HttpChannel channelHttp;
        private IpcChannel channelIpc;

        private IIdentityManagementObjects _idmService;

        public string ProviderName
        {
            get { return "awmremoting"; }
        }

        IDictionary props = new Hashtable();

        public RemotingBotEngine GetRemoteBotInstance()
        {
            return null;            
        }

        public RemoteServices.RemotingBotEngine Login(User user)
        {
            if (user.IsAuthenticated(_idmService.Db))
                return GetRemoteBotInstance();
            return null; // no access to services.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotingServer&lt;TRemotingBotEngine&gt;"/> class.
        /// </summary>
        /// <param name="remotingConnection">The remoting connection.</param>
        /// <param name="idmManagement">The idm management.</param>
        public RemotingServer(string remotingConnection, IIdentityManagementObjects idmManagement)
        {
            _idmService = idmManagement;
                //throw new Exception("The remoting server needs an authentication / authorization manegement object to operate.");
            Connection = new RemotingServerConnection {ConnectionString = remotingConnection};
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
            {
                switch(item.Name.ToLower())
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
                                Connection.Protocol  = RemotingProtocol.Tcp;
                                break;
                            case "http":
                                Connection.Protocol = RemotingProtocol.Http;
                                break;
                            default:
                                throw new Exception(string.Format("Protocol '{0}' is not supported.",item.Value));
                        }
                        break;
                    case "port" :
                        try{Connection.Port = int.Parse(item.Value.Trim());}
                        catch{ThrowIncorrectConnectionStringException();}
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
                throw new ArgumentException("Remoting Server could not be stopped as it is not running.");
            switch (Connection.Protocol)
            {
                case RemotingProtocol.Ipc:
                    channelIpc.StopListening(null);
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
                throw new ArgumentException("Remoting Server already started.");
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
                    ChannelServices.RegisterChannel(channelTcp,false);
                    break;
                case RemotingProtocol.Http:
                    channelHttp = new HttpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channelHttp,false);
                    break;
            }
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TRemotingBotEngine), "RemotingBotEngine", WellKnownObjectMode.Singleton);
            IsRunning = true; // todo make this smarter.
            return true;
        }

        public bool IsRunning
        {
            get; private set;
        }

        #endregion

        #region IConnectionProperties<RemotingServerConnection> Members


        public RemotingServerConnection Connection
        {
            get; private set;
        }

        #endregion

        public string DisplayName
        {
            get { return "Remoting Server"; }
        }

        public Guid Id
        {
            get; internal set;
        }

        public string TechnicalName
        {
            get; internal set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            // TODO: Cleanup.
        }

        #endregion
    }
}