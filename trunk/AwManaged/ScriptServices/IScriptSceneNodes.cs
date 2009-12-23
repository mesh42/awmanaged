using AW;
using AwManaged.Core;
using AwManaged.Huds.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.ScriptServices
{
    public interface IScriptSceneNodes
    {
        ProtectedList<Model> Model { get; }
        ProtectedList<Camera> Cameras { get; }
        ProtectedList<Mover> Movers { get; }
        ProtectedList<ZoneObject> Zones { get; }
        ProtectedList<IHudBase> Huds { get; }
    }
}