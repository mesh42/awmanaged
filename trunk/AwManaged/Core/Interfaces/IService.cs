namespace AwManaged.Core.Interfaces
{
    public interface IService
    {
        bool Stop();
        bool Start();
        bool IsRunning { get; }
    }
}
