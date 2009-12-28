using AwManaged.Core.Interfaces;
using Db4objects.Db4o;

namespace AwManaged.Storage.Interfaces
{
    /// <summary>
    /// Storage Client.
    /// </summary>
    public interface IStorageClient<TConnectionInterface> : IConnection<TConnectionInterface>
    {
        /// <summary>
        /// Gets the Linq queryable db.
        /// </summary>
        /// <value>The db.</value>
        IObjectContainer Db { get; }
        /// <summary>
        /// Opens the connection.
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// Closes the connection.
        /// </summary>
        bool CloseConnection();
    }
}
