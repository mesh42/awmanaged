namespace AwManaged.Logging
{
    public interface ILogger
    {
        void WriteLine(string text);
        void Write(string text);
    }
}