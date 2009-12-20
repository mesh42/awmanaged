using AwManaged.Logging;

namespace AwManaged.Interfaces
{
    public interface ICanLog : IInitialize
    {
        ILogger logger { get; set; }
        void WriteLine(string text);
    }
}