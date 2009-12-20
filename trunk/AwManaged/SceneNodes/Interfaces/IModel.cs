using System;
using AW;
using AwManaged.Math;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface IModel : ICloneable
    {
        int Id { get; set; }
        int Owner { get; set; }
        DateTime Timestamp { get; set; }
        ObjectType Type { get; set; }
        string ModelName { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        string Description { get; set; }
        string Action { get; set; }
        int Number { get; set; }
        new object Clone();
    }
}