using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Services
{
    public abstract class BaseConnection<TConnectionInterface> : IConnection<TConnectionInterface>
    {
        protected BaseConnection(string connection)
        {
            ConnectionString = connection;
        }

        #region IConnection<TConnectionInterface> Members

        public string ConnectionString { get; internal set; }

        #endregion
    }
}
