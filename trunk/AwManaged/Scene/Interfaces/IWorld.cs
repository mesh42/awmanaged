using System;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene.Interfaces
{
    public interface IWorld<T> : ISceneNode<T>
        where T : MarshalByRefObject
    {
        string Name { get; set; }
        Guid Id { get; }
    }
}