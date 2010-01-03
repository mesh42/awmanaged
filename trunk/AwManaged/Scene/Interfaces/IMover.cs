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