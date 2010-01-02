using AwManaged.Core.Interfaces;

namespace AwManaged.RemoteServices
{
    public sealed class RemotingServerConnection : IConnection<RemotingServerConnection>
    {
        public int Port { get; internal set; }
        public RemotingProtocol Protocol { get; internal set; }

        #region IConnection<RemotingServerConnection> Members

        public string ConnectionString
        {
            get; internal set;
        }

        #endregion

        #region IConnectionProviderIdentity Members

        public string ProviderName
        {
            get { return "awmremoting"; }
        }

        #endregion

    }
}
