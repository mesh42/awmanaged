using System.Collections.Generic;
using AW;
using AwManaged.Core;
using AwManaged.Huds.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.Interfaces
{
    public interface ISceneNodes
    {
        ProtectedList<Model> Model { get; set; }
        ProtectedList<Camera> Cameras { get; set; }
        ProtectedList<Mover> Movers { get; set; }
        ProtectedList<ZoneObject> Zones { get; set; }
        ProtectedList<IHudBase> Huds { get; set; }
    }
}
