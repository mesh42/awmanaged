using System;
using AW;
using AwManaged.Math;

namespace AwManaged.Scene.Interfaces
{
    public interface IMover<T> : ISceneNode<T>
        where T : MarshalByRefObject
    {
        sbyte AccelerationTiltX { set; get; }
        sbyte AccelerationTiltZ { set; get; }
        byte AvatarTag { set; get; }
        string BumpName { set; get; }
        MoverFlags Flags { set; get; }
        byte FrictionFactor { set; get; }
        sbyte GlideFactor { set; get; }
        short LockedPitch { set; get; }
        Vector3 LockedPosition { set; get; }
        short LockedYaw { set; get; }
        string Name { set; get; }
        string Script { set; get; }
        string Sequence { set; get; }
        string Sound { set; get; }
        byte SpeedFactor { set; get; }
        byte TurnFactor { set; get; }
        MoverType Type { set; get; }
        System.Collections.Generic.List<Waypoint> Waypoints { get; set; }
    }
}