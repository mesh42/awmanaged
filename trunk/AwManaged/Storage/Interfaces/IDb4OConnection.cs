namespace AwManaged.Storage.Interfaces
{
    public interface IDb4OConnection 
    {
        string HostAddress { get; }
        int HostPort { get; }
        string File { get; }
        string User { get; }
        string Password { get; }
    }
}