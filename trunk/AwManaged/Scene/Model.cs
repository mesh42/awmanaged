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
using System.Xml.Serialization;
using AW;
using AwManaged.Math;
using AwManaged.Scene.Interfaces;
using Db4objects.Db4o.Config.Attributes;

namespace AwManaged.Scene
{
    public sealed class Model : MarshalByRefObject, IModel<Model>
    {
        [Indexed]
        private int _id;
        [Indexed]
        private int _owner;
        [Indexed]
        private DateTime _timestamp;
        [Indexed]
        private ObjectType _type;
        [Indexed]
        private string _modelName;
        [Indexed]
        private Vector3 _position;
        [Indexed]
        private Vector3 _rotation;
        [Indexed]
        private string _description;
        [Indexed]
        private string _action;
        [Indexed]
        private int _number;
        [Indexed]
        private string _data;

        //private int _id;
        //private int _owner;
        //private DateTime _timestamp;
        //private ObjectType _type;
        //private string _modelName;
        //private Vector3 _position;
        //private Vector3 _rotation;
        //private string _description;
        //private string _action;
        //private int _number;
        //private string _data;

        [XmlAttribute]
        public int Id { get { return _id; } internal set{ _id = value;} }
        [XmlAttribute]
        public int Owner { get { return _owner; } set { _owner = value;} }
        [XmlAttribute]
        public DateTime Timestamp { get { return _timestamp;} internal set { _timestamp = value;} }
        [XmlAttribute]
        public ObjectType Type { get { return _type;} internal set { _type = value;} }
        [XmlAttribute]
        public string ModelName { get { return _modelName; } set { _modelName = value;} }
        public Vector3 Position { get { return _position; } set { _position = value;} }
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value;} }
        [XmlAttribute]
        public string Description { get { return _description; } set { _description = value;} }
        [XmlAttribute]
        public string Action { get{ return _action;} set { _action = value;} }
        //[XmlAttribute]
        //public int Number { get { return _number;} internal set { _number = Number;} }
        [XmlAttribute]
        public string Data { get { return _data;} set { _data = value;}  }
        /// <summary>
        /// Indicates if the object is being changed.
        /// </summary>
        internal bool _isChange;
        /// <summary>
        /// Indicates which bot node manages the change to this object.
        /// </summary>
        internal int _ChangeNode;

        public Model(){}

        public Model(int id, int owner, DateTime timestamp, ObjectType type, string model, Vector3 position, Vector3 rotation, string description, string action, string data)
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
            Data = data;
        }

        #region ICloneableT<IModel> Members

        public Model Clone()
        {
            return (Model) MemberwiseClone();
        }

        #endregion

        #region IChanged<Model> Members

        public event AwManaged.Core.Interfaces.ChangedEventDelegate<Model> OnChanged;

        public bool IsChanged{get; internal set;}

        #endregion
    }
}