using System;
using AwManaged.Core.Interfaces;

namespace AwManaged.Scene.Interfaces
{
    public interface ISceneNode<T> : ICloneableT<T>, IChanged<T> where T : MarshalByRefObject
    {
    }
}