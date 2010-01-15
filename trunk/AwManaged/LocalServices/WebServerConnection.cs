using System.Net;
using AwManaged.Core.Services;

namespace AwManaged.LocalServices
{
    public sealed class WebServerConnection : BaseConnection<WebServerConnection>
    {
        public int Port { get; internal set; }
        public IPAddress IpAddress { get; internal set; }

        public WebServerConnection() : base(null)
        {
            
        }

        public WebServerConnection(string connection) : base(connection)
        {
        }

    }
}