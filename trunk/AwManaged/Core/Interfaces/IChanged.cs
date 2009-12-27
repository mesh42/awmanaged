namespace AwManaged.Core.Interfaces
{
    public delegate void ChangedEventDelegate<T>(T sender);

    public interface IChanged<T>
    {
        event ChangedEventDelegate<T> OnChanged;
        bool IsChanged { get; }
    }
}
