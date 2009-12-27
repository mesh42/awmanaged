using System;
using System.ComponentModel;
using Aw.Common.Config;
using AwManaged.Configuration.Interfaces;
using AwManaged.Math;
using AWManaged.Security;

namespace AwManaged.Configuration
{
    [Serializable]
    public class LoginConfiguration : BaseConfiguration<LoginConfiguration>, ILoginConfiguration
    {
        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>The authorization.</value>
        [Description("Authorization for bot administration and debugging purposes.")]
        [Browsable(false)]
        public Authorization Authorization { get; set; }
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        [Description("The IP Address or Fully Qualified Domain Name (FQDN) of the universe server.")]
        [Category("Authentication")]
        public string Domain { get; set; }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        [Description("The TCP port of the universe server.")]
        [Category("Authentication")]
        public int Port { get; set; }
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        [Description("The citizen number of the privilege password")]
        [Category("Authentication")]
        public int Owner { get; set; }
        /// <summary>
        /// Gets or sets the privilege password.
        /// </summary>
        /// <value>The privilege password.</value>
        [Description("The citizens privilege password, who has the appropiate rights to login a bot in the designated world.")]
        [Category("Authentication")]
        public string PrivilegePassword { get; set; }
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        [Description("The name of bot as it appears in the whisper section of the chat window.")]
        [Category("Authentication")]
        public string LoginName { get; set; }
        /// <summary>
        /// Gets or sets the world.
        /// </summary>
        /// <value>The world.</value>
        [Description("The name of the world the bot logs in to.")]
        [Category("Authentication")]
        public string World { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        [Description("The initial position vector of the avatar.")]
        [Category("Positioning")]
        public Vector3 Position { get; set; }
        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        [Description("The initial rotation vector of the avatar, expressed in yaw / pitch and roll.")]
        [Category("Positioning")]
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginConfiguration"/> class.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="port">The port.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="privilegePassword">The privilege password.</param>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="world">The world.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        public LoginConfiguration(Authorization authorization, string domain, int port, int owner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
        {
            Authorization = authorization;
            Domain = domain;
            Port = port;
            Owner = owner;
            PrivilegePassword = privilegePassword;
            LoginName = loginName;
            World = world;
            Position = position;
            Rotation = rotation;
        }

        public LoginConfiguration(Guid id)
        {
            LoginConfiguration loginConfiguration;
            Load(id, out loginConfiguration);
            initialize(loginConfiguration);
        }

        public LoginConfiguration(string name)
        {
            LoginConfiguration loginConfiguration;
            Load(name, out loginConfiguration);
            initialize(loginConfiguration);
        }


        ///// <summary>
        ///// Initializes a new instance of the <see cref="LoginConfiguration"/> class.
        ///// For new configurations.
        ///// </summary>
        //public LoginConfiguration()
        //{
        //    Position = new Vector3(0,0,0);
        //    Rotation = new Vector3(0,0,0);
        //}

        private void initialize(LoginConfiguration loginConfiguration)
        {
            Authorization = loginConfiguration.Authorization;
            Domain = loginConfiguration.Domain;
            Port = loginConfiguration.Port;
            Owner = loginConfiguration.Owner;
            PrivilegePassword = loginConfiguration.PrivilegePassword;
            LoginName = loginConfiguration.LoginName;
            World = loginConfiguration.World;
            Position = loginConfiguration.Position;
            Rotation = loginConfiguration.Rotation;
        }

        public static void Load<T>(Guid id, out T configuration)
        {
            throw new NotImplementedException();
            //using (var db = new AwDataContext())
            //{
            //    var record = from p in db.core_configurations where p.configuration_id == id select p;
            //    if (record.Count() == 1)
            //    {
            //        configuration = (T)Serialization.Deserialize(record.ElementAtOrDefault(0).confgiuration_image.ToArray());
            //    }
            //}
            //configuration = default(T);
        }

        public static void Load<T>(string name, out T configuration)
        {
            throw new NotImplementedException();
            //using (var db = new AwDataContext())
            //{
            //    var record = from p in db.core_configurations where p.configuration_name == name select p;
            //    if (record.Count() == 1)
            //    {
            //        configuration = (T)Serialization.Deserialize(record.ElementAtOrDefault(0).confgiuration_image.ToArray());
            //    }
            //}
            //configuration = default(T);
        }


        public static void Store<T>(Guid id, string name, T configuration)
        {
            throw new NotImplementedException();
            //using (var db = new AwDataContext())
            //{
            //    var record = new core_configuration
            //                     {
            //                         configuration_id = id,
            //                         configuration_name = name,
            //                         configuration_type = configuration.GetType().ToString(),
            //                         confgiuration_image = new Binary(Serialization.Serialize(configuration))
            //                     };
            //    db.core_configurations.InsertOnSubmit(record);
            //    db.SubmitChanges();
            //}
        }

    }
}