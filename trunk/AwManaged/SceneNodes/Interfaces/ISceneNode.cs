using AwManaged.Core.Interfaces;

namespace AwManaged.SceneNodes.Interfaces
{
    public interface ISceneNode<T> : ICloneableT<T>, IChanged<T>
    {
    }
}
