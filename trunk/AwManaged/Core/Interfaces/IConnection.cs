using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Interfaces
{
    public interface IConnection<TConnectionInterface> : IConnectionProviderIdentity
    {
        string ConnectionString { get; }
        TConnectionInterface Connection { get; }
    }
}