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