using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Interfaces
{
    public interface IConnection<TConnectionInterface>
    {
        string ConnectionString { get; }
    }
}