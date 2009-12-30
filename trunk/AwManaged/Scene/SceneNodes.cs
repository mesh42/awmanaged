using System;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    /// <summary>
    /// Exact implementation of scene nodes for active worlds scene objects.
    /// </summary>
    public sealed class SceneNodes : MarshalByRefObject, ISceneNodes<Model, Camera, Mover, Zone, HudBase<Avatar>, Avatar>, ICloneableT<SceneNodes>
    {
        #region ISceneNodes<Model,Camera,Mover,Zone,HudBase<Avatar>,Avatar> Members

        public ProtectedList<Avatar> Avatars
        { get; set; }
        public ProtectedList<Model> Models
        { get; set; }
        public ProtectedList<Camera> Cameras
        { get; set; }
        public ProtectedList<Mover> Movers
        { get; set; }
        public ProtectedList<Zone> Zones
        { get; set; }
        public ProtectedList<HudBase<Avatar>> Huds
        { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodes"/> class.
        /// </summary>
        public SceneNodes()
        {
            Cameras = new ProtectedList<Camera>();
            Movers = new ProtectedList<Mover>();
            Zones = new ProtectedList<Zone>();
            Huds = new ProtectedList<HudBase<Avatar>>();
            Avatars = new ProtectedList<Avatar>();
        }

        #region ICloneableT<SceneNodes> Members

        public SceneNodes Clone()
        {
            //return new SceneNodes {Avatars = Avatars.Clone(), Cameras = Cameras.Clone(), Models = Models.Clone()};
            return (SceneNodes) MemberwiseClone();
        }

        #endregion
    }
}
