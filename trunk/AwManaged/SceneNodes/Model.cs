using System;
using System.Xml.Serialization;
using AW;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public sealed class Model : IModel<Model>
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public int Owner { get; set; }
        [XmlAttribute]
        public DateTime Timestamp { get; set; }
        [XmlAttribute]
        public ObjectType Type { get; set; }
        [XmlAttribute]
        public string ModelName { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public string Action { get; set; }
        [XmlAttribute]
        public int Number { get; set; }
        [XmlAttribute]
        public string Data { get; set; }

        public Model(){}

        public Model(int id, int owner, DateTime timestamp, ObjectType type, string model, Vector3 position, Vector3 rotation, string description, string action, int number, string data)
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
            Number = number;
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
