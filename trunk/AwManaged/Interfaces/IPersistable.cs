namespace AwManaged.Interfaces
{
    public interface IPersistable<T>
    {
        T Load();
        void Save();
    }
}