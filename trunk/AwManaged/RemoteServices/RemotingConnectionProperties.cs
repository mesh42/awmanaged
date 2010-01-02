using AwManaged.Core.Interfaces;

namespace AwManaged.RemoteServices
{
    public class RemotingConnectionProperties : ICloneableT<RemotingConnectionProperties>
    {
        public RemotingConnectionProperties Clone()
        {
            return (RemotingConnectionProperties) MemberwiseClone();
        }
    }
}
