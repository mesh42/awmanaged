using System;
using System.Xml.Serialization;
using AW;
using AwManaged.Math;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.SceneNodes
{
    public class Model : IModel
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

        #region ICloneable Members

        public object ClonePrecise()
        {
            return new Model(Id, Owner, DateTime.Now, ObjectType.V3, ModelName, new Vector3(Position.x, Position.y, Position.z), new Vector3(Rotation.x, Rotation.y, Rotation.z), Description, Action, Number, Data);
        }

        public object Clone()
        {
            return new Model(0, 0, DateTime.Now, ObjectType.V3, ModelName, new Vector3(Position.x, Position.y, Position.z), new Vector3(Rotation.x, Rotation.y, Rotation.z), Description, Action, 0, Data);
        }

        #endregion
    }
}

//Argument attributes

//AW_OBJECT_OWNER
//AW_OBJECT_BUILD_TIMESTAMP
//If set to 0 (zero) then the world will use its current local time.
//AW_OBJECT_TYPE
//AW_OBJECT_X
//AW_OBJECT_Y
//AW_OBJECT_Z
//AW_OBJECT_YAW
//AW_OBJECT_TILT
//AW_OBJECT_ROLL
//AW_OBJECT_DESCRIPTION
//AW_OBJECT_ACTION
//AW_OBJECT_MODEL
//AW_OBJECT_DATA
//AW_OBJECT_CALLBACK_REFERENCE