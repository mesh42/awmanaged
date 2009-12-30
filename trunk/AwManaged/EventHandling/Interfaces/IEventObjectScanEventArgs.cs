using AwManaged.Core.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    interface IEventObjectScanEventArgs<TSceneNodes> where TSceneNodes : ICloneableT<TSceneNodes>
    {
        TSceneNodes SceneNodes { get; }
    }
}
