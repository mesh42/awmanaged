using System;

namespace AwManaged.Scene.Interfaces
{
    public interface IParticleFlags<T> : ISceneNode<T> where T : MarshalByRefObject
    {
        bool CameraEmit { get; set; }
        bool DrawInFront { get; set; }
        bool Gravity { get; set; }
        bool Interpolate { get; set; }
        bool LinkToMover { get; set; }
        bool ZoneCollision { get; set; }
        bool ZoneExclusive { get; set; }
    }
}