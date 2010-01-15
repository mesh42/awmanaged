using AwManaged.Core.Services;

namespace AwManaged.RemoteServices
{
    public sealed class RemotingBotServerConnection : BaseConnection<RemotingBotServerConnection>
    {
        public int Port { get; internal set; }
        public RemotingProtocol Protocol {get;internal set;}

        public RemotingBotServerConnection()
            : base(null)
        {

        }

        public RemotingBotServerConnection(string connection)
            : base(connection)
        {
        }

    }

}
