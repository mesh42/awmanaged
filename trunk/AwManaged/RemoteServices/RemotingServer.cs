using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
using AwManaged.Core;
using AwManaged.RemoteServices.Interfaces;

namespace AwManaged.RemoteServices
{
    public class RemotingServer<TRemotingBotEngine> : IRemotingServer<RemotingServerConnection>
    {
        private BinaryClientFormatterSinkProvider clientProvider;
        private BinaryServerFormatterSinkProvider serverProvider;
        private TcpChannel channelTcp;
        private HttpChannel channelHttp;
        IDictionary props = new Hashtable();

        public RemotingServer(string connectionString)
        {
            Connection = new RemotingServerConnection {ConnectionString = connectionString};
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, Connection.ProviderName))
            {
                switch(item.Name.ToLower())
                {
                    case "provider":
                        break; // handled by the ConnectionStringHelper
                    case "protocol":
                        switch (item.Value.ToLower().ToLower())
                        {
                            case "tcp":
                                Connection.Protocol  = RemotingProtocol.tcp;
                                break;
                            case "http":
                                Connection.Protocol = RemotingProtocol.http;
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
                if (Connection.Protocol == RemotingProtocol.unspecified)
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
                case RemotingProtocol.tcp:
                    channelTcp.StopListening(null);
                    ChannelServices.UnregisterChannel(channelTcp);
                    break;
                case RemotingProtocol.http:
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
                case RemotingProtocol.tcp:
                    channelTcp = new TcpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channelTcp,false);
                    break;
                case RemotingProtocol.http:
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
    }
}
