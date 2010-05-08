/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using SharedMemory;using System;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Patterns;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    /// <summary>
    /// Exact implementation of scene nodes for active worlds scene objects.
    /// </summary>
    [Serializable]
    public sealed class SceneNodes : MarshalIndefinite, ISceneNodes<Model, Camera, Mover, Zone, HudBase<Avatar>, Avatar,Particle,ParticleFlags>, ICloneableT<SceneNodes>
    {
        #region ISceneNodes<Model,Camera,Mover,Zone,HudBase<Avatar>,Avatar> Members

        public ProtectedList<Avatar> Avatars{ get; set; }
        public ProtectedList<Model> Models{ get; set; }
        public ProtectedList<Camera> Cameras{ get; set; }
        public ProtectedList<Mover> Movers{ get; set; }
        public ProtectedList<Zone> Zones{ get; set; }
        public ProtectedList<HudBase<Avatar>> Huds{ get; set; }
        public ProtectedList<Particle> Particles{ get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNodes"/> class.
        /// </summary>
        public SceneNodes()
        {
            Models = new ProtectedList<Model>();
            Cameras = new ProtectedList<Camera>();
            Movers = new ProtectedList<Mover>();
            Zones = new ProtectedList<Zone>();
            Huds = new ProtectedList<HudBase<Avatar>>();
            Avatars = new ProtectedList<Avatar>();
            Particles = new ProtectedList<Particle>();
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
