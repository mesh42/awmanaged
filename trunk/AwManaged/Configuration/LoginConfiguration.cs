using System;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
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

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LoginConfiguration"/> class.
        ///// </summary>
        ///// <param name="authorization">The authorization.</param>
        ///// <param name="domain">The domain.</param>
        ///// <param name="port">The port.</param>
        ///// <param name="owner">The owner.</param>
        ///// <param name="privilegePassword">The privilege password.</param>
        ///// <param name="loginName">Name of the login.</param>
        ///// <param name="world">The world.</param>
        ///// <param name="position">The position.</param>
        ///// <param name="rotation">The rotation.</param>
        //public LoginConfiguration(Authorization authorization, string domain, int port, int owner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
        //{
        //    Authorization = authorization;
        //    Domain = domain;
        //    Port = port;
        //    Owner = owner;
        //    PrivilegePassword = privilegePassword;
        //    LoginName = loginName;
        //    World = world;
        //    Position = position;
        //    Rotation = rotation;
        //}

        //private void initialize(LoginConfiguration loginConfiguration)
        //{
        //    Authorization = loginConfiguration.Authorization;
        //    Domain = loginConfiguration.Domain;
        //    Port = loginConfiguration.Port;
        //    Owner = loginConfiguration.Owner;
        //    PrivilegePassword = loginConfiguration.PrivilegePassword;
        //    LoginName = loginConfiguration.LoginName;
        //    World = loginConfiguration.World;
        //    Position = loginConfiguration.Position;
        //    Rotation = loginConfiguration.Rotation;
        //}

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

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}