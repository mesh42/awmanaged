using System;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface IWorld
    {
        string Name { get; set; }
        Guid Id { get; }
    }
}