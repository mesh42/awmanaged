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
using System;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Services;
using Db4objects.Db4o;

namespace AwManaged.Storage
{
    /// <summary>
    /// Connectionstring format for db4o is in this format:
    /// 
    /// "provider=db4o;user=awmanaged;password=awmanaged;port=4571;file=awmanaged.dat"
    /// </summary>
    public sealed class Db4OStorageServer : IConnectedServiceDevice<Db4OConnection>
    {
        private IObjectServer _server;
        public string ProviderName { get { return "db4o"; } }

        public Db4OStorageServer(string connectionString)
        {
            Connection = new Db4OConnection {ConnectionString = connectionString};
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
            {
                switch(item.Name.ToLower())
                {
                    case "host":
                        break; // not used for server.
                    case "provider":
                        break; // handled by the ConnectionStringHelper
                    case "user":
                        Connection.User = item.Value.Trim();
                        break;
                    case "password" :
                        Connection.Password = item.Value.Trim();
                        break;
                    case "file" :
                        Connection.File = item.Value.Trim();
                        break;
                    case "port" :
                        try{Connection.HostPort = int.Parse(item.Value.Trim());}
                        catch{ThrowIncorrectConnectionStringException();}
                        break;
                    default:
                        ThrowIncorrectConnectionStringException();
                        break;
                }
            }
            EvaluateConnectionProperties();
        }

        private void EvaluateConnectionProperties()
        {
                if (Connection.User == null || Connection.Password == null || Connection.User == string.Empty || Connection.Password == string.Empty)
                    throw new ArgumentException("Connection string does not contain mandatory authentication credentials.");
                if (Connection.File == null || Connection.File == string.Empty)
                    throw new ArgumentException("Connection string does not contain mandatory data file name.");
                if (Connection.HostPort <= 0)
                    throw new ArgumentException("A port number has not been specified or should be a positive number.");
        }

        static void ThrowIncorrectConnectionStringException()
        {
            throw new ArgumentException("Connectionstring is in the incorrect format");
        }

        #region IService Members

        public bool Stop()
        {
            return IsRunning && _server.Close();
        }

        public bool Start()
        {
            if (IsRunning)
                throw new ArgumentException("Storage server already started.");
            EvaluateConnectionProperties();
            _server = Db4oFactory.OpenServer(Connection.File,Connection.HostPort);
            _server.GrantAccess(Connection.User, Connection.Password);
            return true;
        }

        public bool IsRunning
        {
            get { return _server != null; }
        }

        #endregion

        #region IStorageConfiguration<Db4OConnection> Members

        public Db4OConnection Connection
        {
            get; internal set;
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get { return "Storage Server."; }
        }

        public Guid IdentifyableId
        {
            get; internal set;
        }

        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _server.Close();
        }

        #endregion
    }
}
