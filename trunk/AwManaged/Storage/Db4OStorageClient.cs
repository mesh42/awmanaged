/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using SharedMemory;using System;
using AwManaged.Core.Services;
using AwManaged.Storage.Interfaces;
using Db4objects.Db4o;

namespace AwManaged.Storage
{
    /// <summary>
    /// Metbase clients, connects to a database server when storing objects.
    /// </summary>
    public class Db4OStorageClient : IStorageClient<Db4OConnection>
    {
        public Db4OStorageClient(string connectionString)
        {
            Connection = new Db4OConnection {ConnectionString = connectionString};
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
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
                        catch { ConnectionStringHelper.ThrowIncorrectConnectionString(Connection.ConnectionString); }
                        break;
                    default:
                        ConnectionStringHelper.ThrowIncorrectConnectionString(Connection.ConnectionString);
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

        #region IHaveToCleanUpMyShit Members

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

        #region IStorageConfiguration<IDb4OConnection> Members

        public Db4OConnection Connection
        {
            get; internal set;
        }

        #endregion

        #region IService Members

        public bool Stop()
        {
            _safeObjectContainer.Dispose();
            return true;
        }

        public bool Start()
        {
            _safeObjectContainer = new ThreadSafeObjectContainer<Db4OConnection>(Connection);
            return true;
        }

        public bool IsRunning
        {
            get { return _safeObjectContainer != null; }
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get { return "Storage Client";}
        }

        public Guid IdentifyableId
        {
            get { throw new NotImplementedException(); }
        }

        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion

        #region ICloneableT<IObjectContainer> Members

        public IObjectContainer Clone()
        {
            return _safeObjectContainer.GetNewInstance();
        }

        #endregion
    }
}