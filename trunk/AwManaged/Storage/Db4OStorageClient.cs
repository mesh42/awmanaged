﻿using System;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.Storage.Interfaces;
using Db4objects.Db4o;

namespace AwManaged.Storage
{
    /// <summary>
    /// Metbase clients, connects to a database server when storing objects.
    /// </summary>
    public class Db4OStorageClient : IStorageClient<Db4OConnection>
    {
        public Db4OStorageClient(IConnection<Db4OConnection> storageConfiguration)
        {
            ConnectionString = storageConfiguration.ConnectionString;
            Connection = new Db4OConnection();
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(ConnectionString, ProviderName))
            {
                switch (item.Name.ToLower())
                {
                    case "file":
                        break; // not used by the client.
                    case "provider":
                        break; // handled by the connection string helper.
                    case "user":
                        Connection.User = item.Value.Trim();
                        break;
                    case "password":
                        Connection.Password = item.Value.Trim();
                        break;
                    case "server":
                        Connection.HostAddress = item.Value.Trim();
                        break;
                    case "port":
                        try { Connection.HostPort = int.Parse(item.Value.Trim()); }
                        catch { ConnectionStringHelper.ThrowIncorrectConnectionString(ConnectionString); }
                        break;
                    default:
                        ConnectionStringHelper.ThrowIncorrectConnectionString(ConnectionString);
                        break;
                }
            }
            EvaluateConnectionProperties();
        }

        private void EvaluateConnectionProperties()
        {
            if (Connection.User == null || Connection.Password == null || Connection.User == string.Empty || Connection.Password == string.Empty)
                throw new ArgumentException("Connection string does not contain mandatory authentication credentials.");
            if (Connection.HostAddress == null || Connection.HostAddress == string.Empty)
                throw new ArgumentException("Connection string does not contain mandatory host address.");
            if (Connection.HostPort <= 0)
                throw new ArgumentException("A port number has not been specified or should be a positive number.");
        }

        private ThreadSafeObjectContainer<Db4OConnection> _safeObjectContainer;
        /// <summary>
        /// Opens the connection on the current thread by default.
        /// </summary>
        public void OpenConnection()
        {
            _safeObjectContainer = new ThreadSafeObjectContainer<Db4OConnection>(Connection);
        }

        /// <summary>
        /// Closes the connection on the current thread.
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            return _safeObjectContainer.RemoveInstance();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _safeObjectContainer.Dispose();
        }

        #endregion

        #region IStorageClient Members

        // make thread safe, so we can spawn Atomic transactions over multiple threads.


        public IObjectContainer Db
        {
            get { return _safeObjectContainer.GetInstance(); }
        }

        #endregion

        #region IProviderIdentity Members

        public string ProviderName
        {
            get { return "db4o"; }
        }

        #endregion

        #region IStorageConfiguration Members

        public string ConnectionString
        {
            get; private set;
        }

        #endregion

        #region IStorageConfiguration<IDb4OConnection> Members

        public Db4OConnection Connection
        {
            get; internal set;
        }

        #endregion

        #region IStorageClient<Db4OConnection> Members


        bool IStorageClient<Db4OConnection>.CloseConnection()
        {
            return Db.Close();
        }

        #endregion


        #region ICloneableT<IObjectContainer> Members

        IObjectContainer ICloneableT<IObjectContainer>.Clone()
        {
            return _safeObjectContainer.GetNewInstance();
        }

        #endregion
    }
}