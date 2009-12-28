using AwManaged.Core.Interfaces;

namespace AwManaged.Storage.Interfaces
{
    public interface IStorageServer<TConnectionInterface> : IConnection<TConnectionInterface>, IService
    {
    }
}
