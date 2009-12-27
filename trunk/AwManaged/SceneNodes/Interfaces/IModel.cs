using System;
using AW;
using AwManaged.Math;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface IModel<T> : ISceneNode<T>
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
        string Data { get; set; }
        int Number { get; set; }
    }
}