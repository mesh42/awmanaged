using System;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface IWorld<T> : ISceneNode<T>
    {
        string Name { get; set; }
        Guid Id { get; }
    }
}