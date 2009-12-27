namespace AwManaged.Core.Interfaces
{
    /// <summary>
    /// Indicates an object template is cloneable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneableT<T>
    {
        /// <summary>
        /// Clones this instance to prevent dirty reads and writes on the original object.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}