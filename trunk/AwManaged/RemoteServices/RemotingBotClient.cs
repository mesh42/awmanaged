using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace AwManaged.RemoteServices
{
    [Serializable]
    public abstract class RemotingBotClient
    {
        public BotEngine Remote;

        public RemotingBotClient(string server, int port)
        {
            ChannelServices.RegisterChannel(new HttpChannel(0));
            RemotingConfiguration.RegisterWellKnownClientType(typeof(BotEngine),"http://localhost:9000/RemotingBotClient");
            //Remote = new BotEngine();
            //Remote = (BotEngine)Activator.GetObject(typeof(BotEngine), string.Format("http://{0}:{1}/48fb0125_6188_4dff_95ea_d1b55f7a9a4c/RemotingBotClient", server, port));
        }
    }
}
