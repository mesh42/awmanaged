using AwManaged.Core.Interfaces;

namespace AwManaged.RemoteServices.Interfaces
{
    public interface IRemotingServer<TConnectionInterface> : IService, IConnectionProperties<TConnectionInterface> where TConnectionInterface : IConnection<TConnectionInterface>
    {
    }
}
