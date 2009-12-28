using AwManaged.Core.Interfaces;

namespace AwManaged.Storage
{
    public sealed class StorageConfiguration<TConnectionInterface> : IConnection<TConnectionInterface>
    {

        public string ConnectionString
        {
            get; private set;
        }

        public StorageConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region IStorageConfiguration<TConnectionInterface> Members


        public TConnectionInterface Connection
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region IStorageProviderIdentity Members

        public string ProviderName
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
