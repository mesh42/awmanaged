using SharedMemory;using System;
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
    public class RemotingServer<TRemotingBotEngine> : BaseConnectedServiceDevice<RemotingServerConnection>
    {
        private BinaryClientFormatterSinkProvider clientProvider;
        private BinaryServerFormatterSinkProvider serverProvider;
        private TcpChannel channelTcp;
        private HttpChannel channelHttp;
        private IpcChannel channelIpc;

        private IIdentityManagementObjects _idmService;

        public override string ProviderName
        {
            get { return "awmremoting"; }
        }

        IDictionary props = new Hashtable();

        public RemotingBotEngine GetRemoteBotInstance()
        {
            return null;            
        }

        public RemotingServer(string connection) : base(connection)
        {
        }

        public RemoteServices.RemotingBotEngine Login(User user)
        {
            if (user.IsAuthenticated(_idmService.Db))
                return GetRemoteBotInstance();
            return null; // no access to services.
        }

        public RemotingServer(string remotingConnection, IIdentityManagementObjects idmManagement) : base(remotingConnection)
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

        internal override void EvaluateConnectionProperties()
        {
            if (Connection.Protocol == RemotingProtocol.Unspecified)
                throw new ArgumentException("Connection string does not contain mandatory protocol type.");
            if (Connection.Port <= 0)
                throw new ArgumentException("A port number has not been specified or should be a positive number.");
        }


        #region IService Members

        public bool Stop()
        {
            base.Stop();
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
            base.Start();
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
            return true;
        }

        #endregion

        #region IConnectionProperties<RemotingServerConnection> Members


        public RemotingServerConnection Connection
        {
            get; private set;
        }

        #endregion

        public override string IdentifyableDisplayName
        {
            get { return "Remoting Server"; }
        }

        public override Guid IdentifyableId
        {
            get { return new Guid("{94E26F19-9255-41c5-9CFC-0C9EDFE33E07}"); }
        }

        public override string IdentifyableTechnicalName
        {
            get; set;
        }


        #region IDisposable Members

        public override void Dispose()
        {
            Stop();
        }

        #endregion
    }
}