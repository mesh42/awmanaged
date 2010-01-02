namespace AwManaged.Core.Interfaces
{
    public interface IConnectionProperties<TConnectionInterface> where TConnectionInterface : IConnection<TConnectionInterface>
    {
        TConnectionInterface Connection { get; }
    }
}
