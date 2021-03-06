﻿/* **********************************************************************************
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
using System.ComponentModel;
using AwManaged.Configuration.Interfaces;
using AwManaged.Core.Interfaces;
using AwManaged.Math;
using AWManaged.Security;

namespace AwManaged.Configuration
{
    public class UniverseConnectionProperties : MarshalIndefinite, IUniverseConnectionProperties<UniverseConnectionProperties>
    {
        private int _connectionTimeOut = 10000;

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
        [Category("Connection")]
        public string Domain { get; set; }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        [Description("The TCP port of the universe server.")]
        [Category("Connection")]
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
        /// Gets or sets the connection timeout.
        /// </summary>
        /// <value>The connection timeout.</value>
        [Description("The number of milliseconds before a connection time out is reached.")]
        [Category("Connection")]
        public int ConnectionTimeOut { get { return _connectionTimeOut; } set {_connectionTimeOut = value ; }}


        #region IUniverseConnectionProperties<UniverseConnectionProperties> Members

        Authorization IUniverseConnectionProperties<UniverseConnectionProperties>.Authorization
        {
            get { throw new System.NotImplementedException(); }
        }

        string IUniverseConnectionProperties<UniverseConnectionProperties>.Domain
        {
            get { throw new System.NotImplementedException(); }
        }

        int IUniverseConnectionProperties<UniverseConnectionProperties>.Port
        {
            get { throw new System.NotImplementedException(); }
        }

        int IUniverseConnectionProperties<UniverseConnectionProperties>.Owner
        {
            get { throw new System.NotImplementedException(); }
        }

        string IUniverseConnectionProperties<UniverseConnectionProperties>.PrivilegePassword
        {
            get { throw new System.NotImplementedException(); }
        }

        string IUniverseConnectionProperties<UniverseConnectionProperties>.LoginName
        {
            get { throw new System.NotImplementedException(); }
        }

        string IUniverseConnectionProperties<UniverseConnectionProperties>.World
        {
            get { throw new System.NotImplementedException(); }
        }

        Vector3 IUniverseConnectionProperties<UniverseConnectionProperties>.Position
        {
            get { throw new System.NotImplementedException(); }
        }

        Vector3 IUniverseConnectionProperties<UniverseConnectionProperties>.Rotation
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region ICloneableT<UniverseConnectionProperties> Members

        #endregion

        #region ICloneableT<UniverseConnectionProperties> Members

        public UniverseConnectionProperties Clone()
        {
            return (UniverseConnectionProperties) MemberwiseClone();
        }

        #endregion

        #region IIdentifiable Members

        [Browsable(false)]
        public string IdentifyableDisplayName
        {
            get; set;
        }

        [Browsable(false)]
        public Guid IdentifyableId
        {
            get; set;
        }

        [Browsable(false)]
        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion
    }
}
