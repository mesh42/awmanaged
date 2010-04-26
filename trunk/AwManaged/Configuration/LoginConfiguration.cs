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
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Services;
using AwManaged.Math;
using AWManaged.Security;

namespace AwManaged.Configuration
{
    /// <summary>
    /// Build a new universe connection property object based on a connection string in the following format:
    /// 
    /// provider=aw;domain=yourdomain;port=7100;login owner:0;privilege password:yourpassword;login name:testbot;world=yourworld;position=50,49,48;rotation=38,39,40
    /// </summary>
    public sealed class LoginConfiguration :  IConnection<UniverseConnectionProperties>, IConfiguration
    {
        public LoginConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new UniverseConnectionProperties {Authorization = new Authorization()};
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(ConnectionString, ProviderName))
            {
                switch (item.Name.ToLower())
                {
                    case "provider":
                        break; // handled by the connection string helper.
                    case "world":
                        Connection.World = item.Value.Trim();
                        break;
                    case "login owner" :
                        try { Connection.Owner = int.Parse(item.Value.Trim()); }
                        catch { ConnectionStringHelper.ThrowIncorrectConnectionString(ConnectionString); }
                        break;
                    case "login name":
                        Connection.LoginName = item.Value.Trim();
                        break;
                    case "privilege password":
                        Connection.PrivilegePassword = item.Value.Trim();
                        break;
                    case "domain":
                        Connection.Domain = item.Value.Trim();
                        break;
                    case "port":
                        try { Connection.Port = int.Parse(item.Value.Trim()); }
                        catch { ConnectionStringHelper.ThrowIncorrectConnectionString(ConnectionString); }
                        break;
                    case "position" :
                        Connection.Position = Vector3.Parse(item.Value.Trim());
                        break;
                    case "rotation":
                        Connection.Rotation = Vector3.Parse(item.Value.Trim());
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
            if (Connection.LoginName == null || Connection.PrivilegePassword == null ||
                Connection.LoginName == string.Empty || Connection.PrivilegePassword == string.Empty ||
                Connection.Owner <= 0)
                throw new ArgumentException("Connection string does not contain mandatory authentication credentials.");
            if (Connection.Domain == null || Connection.Domain == string.Empty)
                throw new ArgumentException("Connection string does not contain mandatory host address.");
            if (Connection.Port <= 0)
                throw new ArgumentException("A port number has not been specified or should be a positive number.");
        }

        #region IConnectionProviderIdentity Members

        public string ProviderName
        {
            get { return "aw"; }
        }

        #endregion

        #region IConnection<UniverseConnectionProperties> Members

        public string ConnectionString
        {
            get; private set;
        }

        public UniverseConnectionProperties Connection
        {
            get; private set;
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get; internal set;
        }

        public Guid IdentifyableId
        {
            get; internal set;
        }

        public string IdentifyableTechnicalName
        {
            get; internal set;
        }

        #endregion
    }
}