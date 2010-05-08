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
using AwManaged.Core.Interfaces;
using AwManaged.Storage.Interfaces;
using AwManaged.Scene;
using Db4objects.Db4o.Config.Attributes;

namespace AwManaged.Storage.BackupProvider
{
    /// <summary>
    /// Backup types
    /// </summary>
    public enum BackupType
    {
        /// <summary>
        /// Full backup, world wide. Currently the only supported backup type.
        /// </summary>
        Full
    }

    /// <summary>
    /// Contains one backup record and static methods to backup a record.
    /// To get a backup set, use a simple linq query else-where to select a backup record.
    /// </summary>
    public sealed class BackupRecord
    {
        #region Fields

        [Indexed]
        private readonly Guid _id;
        [Indexed]
        private readonly string _world;
        [Indexed]
        private readonly string _label;
        private readonly string _description;
        [Indexed]
        private readonly int _citizen;
        [Indexed]
        private readonly DateTime _date;
        [Indexed]
        private readonly BackupType _backupType;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the unique id of the backup.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id { get { return _id; } }
        /// <summary>
        /// Gets the description of the backup.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get { return _description; } }
        /// <summary>
        /// Gets the label, which can help identify a backup.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get { return _label; } }
        /// <summary>
        /// Gets the world name that was part of the backup.
        /// </summary>
        /// <value>The world.</value>
        public string World { get { return _world; } }
        /// <summary>
        /// Gets the citizen who performed the backup.
        /// </summary>
        /// <value>The citizen.</value>
        public int Citizen { get { return _citizen; } }
        /// <summary>
        /// Gets or sets the scene nodes.
        /// </summary>
        /// <value>The scene nodes.</value>
        public Scene.SceneNodes SceneNodes { get; private set; }
        /// <summary>
        /// Gets the type of the backup.
        /// </summary>
        /// <value>The type of the backup.</value>
        public BackupType BackupType { get { return _backupType; } }
        /// <summary>
        /// Gets the date and time of the backup.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get { return _date; } }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupRecord"/> class.
        /// </summary>
        /// <param name="backupType">Type of the backup.</param>
        /// <param name="world">The world.</param>
        /// <param name="citizen">The citizen.</param>
        /// <param name="label">The label.</param>
        /// <param name="description">The description.</param>
        /// <param name="sceneNodes">The scene nodes.</param>
        public BackupRecord(BackupType backupType, string world, int citizen, string label, string description, SceneNodes sceneNodes)
        {
            _world = world;
            _id = Guid.NewGuid();
            _citizen = citizen;
            _label = label;
            _description = description;
            _date= DateTime.Now;
            SceneNodes = sceneNodes;
            _backupType = backupType;
        }

        /// <summary>
        /// Backups the specified world to a medium using the specified storage client.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="storageClient">The storage client.</param>
        /// <param name="backupRecord">The backup record.</param>
        public static void Backup<TClient>(IStorageClient<TClient> storageClient, BackupRecord backupRecord)
            where TClient : IConnection<TClient>
        {
            // long running process, use an isolated db connection.
            using (var db = storageClient.Clone())
            {
                db.Store(backupRecord);
                db.Commit();
            } // disposes te db connection.
        }
     }
}