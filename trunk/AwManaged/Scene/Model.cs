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
using System.ComponentModel;
using System.Xml.Serialization;
using AW;
using AwManaged.Core.Interfaces;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;
using Db4objects.Db4o.Config.Attributes;

namespace AwManaged.Scene
{
    public sealed class Model : MarshalByRefObject, IModel<Model>
    {
        [Indexed] private int _id;
        [Indexed] private int _owner;
        [Indexed] private DateTime _timestamp;
        [Indexed] private ObjectType _type;
        [Indexed] private string _modelName;
        [Indexed] private Vector3 _position;
        [Indexed] private Vector3 _rotation;
        [Indexed] private string _description;
        [Indexed] private string _action;
        [Indexed] private int _number;

        [Browsable(true)]
        [Category("Identification")]
        [Description("A vector containing the x and z coordinates of the cell this object is currently in.")]
        [ReadOnly(true)]
        public int Id { get { return _id; } internal set { _id = value; } }
        [Browsable(true)]
        [Category("Identification")]
        [Description("The citizen number of the owner of this object")]
        [ReadOnly(true)]
        public int Owner { get { return _owner; } set { _owner = value; } }
        [Browsable(true)]
        [Category("Other")]
        [Description("A vector containing the x and z coordinates of the cell this object is currently in.")]
        [ReadOnly(true)]
        public DateTime Timestamp { get { return _timestamp; } internal set { _timestamp = value; } }
        [Browsable(true)]
        [Category("Behavior")]
        [Description("The type of the object")]
        [ReadOnly(true)]
        public ObjectType Type { get { return _type; } internal set { _type = value; } }
        [Browsable(true)]
        [Category("Behavior")]
        [Description("The RWX model name of the object")]
        public string ModelName { get { return _modelName; } set { _modelName = value; } }
        [Browsable(true)]
        [Category("Positioning")]
        [Description("The position vector of the object.")]
        public Vector3 Position { get { return _position; } set { _position = value; } }
        [Browsable(true)]
        [Category("Positioning")]
        [Description("The rotation vector of the object.")]
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; } }
        [Browsable(true)]
        [Category("Identification")]
        [Description("The description of the object.")]
        public string Description { get { return _description; } set { _description = value; } }
        [Browsable(true)]
        [Category("Behavior")]
        [Description("The action string of the object")]
        public string Action { get { return _action; } set { _action = value; } }

        /// <summary>
        /// Indicates if the object is being changed.
        /// </summary>
        internal bool _isChange;
        /// <summary>
        /// Indicates which bot node manages the change to this object.
        /// </summary>
        internal int _ChangeNode;

        public Model()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;

        }

        public Model(int id, int owner, DateTime timestamp, ObjectType type, string model, Vector3 position, Vector3 rotation, string description, string action)
        {
            Id = id;
            Owner = owner;
            Timestamp = timestamp;
            Type = type;
            ModelName = model;
            Position = position;
            Rotation = rotation;
            Description = description;
            Action = action;
            //Number = number;
        }

        #region ICloneableT<IModel> Members

        public Model Clone()
        {
            return (Model) MemberwiseClone();
        }

        #endregion

        #region IChanged<Model> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<Model> OnChanged;

        [Browsable(false)]
        public bool IsChanged { get; internal set; }

        #endregion

        #region ITransactionItem Members

        [Browsable(false)]
        public int Hash
        {
            get; set;
        }

        #endregion

        #region ITransactionItem Members

        [Browsable(false)]
        public bool IsComitted
        {
            get; set;
        }

        #endregion

        #region ITransactionItem Members


        [Browsable(false)]
        public TransactionItemType TransactionItemType
        {
            get; internal set;
        }

        #endregion

        #region ITransactionItem Members


        [Browsable(false)]
        public int TransactionId
        {
            get; set;
        }

        #endregion

        #region ITransactionItem Members

        [Browsable(false)]
        public bool IsRuntimeTransaction
        {
            get;set;
        }

        #endregion


        #region IIdentifiable Members

        [Browsable(false)]
        public string IdentifyableDisplayName
        {
            get { return _id.ToString(); }
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

        [Browsable(true)]
        [Category("Positioning")]
        [Description("A vector containing the x and z coordinates of the cell this object is currently in.")]
        public Vector3 Cell
        {
            get { return new Vector3((int) Position.x/1000, 0, (int) Position.z/1000); }
        }

    }
}