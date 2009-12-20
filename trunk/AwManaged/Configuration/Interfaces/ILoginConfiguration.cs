using System;
using System.ComponentModel;
using AwManaged.Math;
using AWManaged.Security;

namespace AwManaged.Configuration.Interfaces
{
    /// <summary>
    /// Login Configuration for a bot.
    /// </summary>
    public interface ILoginConfiguration
    {
        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>The authorization.</value>
        [Description("Authorization for bot administration and debugging purposes.")]
        [Browsable(false)]
        Authorization Authorization { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        [Description("The IP Address or Fully Qualified Domain Name (FQDN) of the universe server.")]
        [Category("Authentication")]
        string Domain { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        [Description("The TCP port of the universe server.")]
        [Category("Authentication")]
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        [Description("The citizen number of the privilege password")]
        [Category("Authentication")]
        int Owner { get; set; }

        /// <summary>
        /// Gets or sets the privilege password.
        /// </summary>
        /// <value>The privilege password.</value>
        [Description("The citizens privilege password, who has the appropiate rights to login a bot in the designated world.")]
        [Category("Authentication")]
        string PrivilegePassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        [Description("The name of bot as it appears in the whisper section of the chat window.")]
        [Category("Authentication")]
        string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the world.
        /// </summary>
        /// <value>The world.</value>
        [Description("The name of the world the bot logs in to.")]
        [Category("Authentication")]
        string World { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        [Description("The initial position vector of the avatar.")]
        [Category("Positioning")]
        Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        [Description("The initial rotation vector of the avatar, expressed in yaw / pitch and roll.")]
        [Category("Positioning")]
        Vector3 Rotation { get; set; }

        [Description("Displayed name of the object for user interface purposes.")]
        [Category("Identification")]
        [ReadOnly(true)]
        string DisplayName { get; set; }

        [Description("Globally unique identification of the object.")]
        [Category("Identifiaction")]
        [ReadOnly(true)]
        Guid Id { get; set; }
    }
}