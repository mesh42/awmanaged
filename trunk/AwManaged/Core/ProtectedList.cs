using AwManaged.Core.Interfaces;

namespace AwManaged.Core
{
    /// <summary>
    /// Provides a list with minimal functionality exposed to prevent abuse of the AWManaged cache's.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ProtectedList<T> : BaseCacheList<T,ProtectedList<T>> where T : ICloneableT<T>
    {
        public override ProtectedList<T> Clone()
        {
            return (ProtectedList<T>) MemberwiseClone();
        }
    }
}
