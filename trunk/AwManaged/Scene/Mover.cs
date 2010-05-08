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
using AwManaged.Math;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public sealed class Mover : MarshalIndefinite, IMover<Mover>
    {
        #region ICloneableT<Mover> Members

        public Mover Clone()
        {
            return (Mover) MemberwiseClone();
        }

        #endregion

        #region IMover<Mover> Members

        public sbyte AccelerationTiltX{get; set;}
        public sbyte AccelerationTiltZ{get;set;}
        public byte AvatarTag{get;set;}
        public string BumpName{get;set;}
        public AW.MoverFlags Flags{get;set;}
        public byte FrictionFactor{get;set;}
        public sbyte GlideFactor{get;set;}
        public short LockedPitch{get;set;}
        public Vector3 LockedPosition{get;set;}
        public short LockedYaw{get;set;}
        public string Name{get;set;}
        public string Script{get;set;}
        public string Sequence{get;set;}
        public string Sound{get;set;}
        public byte SpeedFactor{get;set;}
        public byte TurnFactor{get;set;}
        public AW.MoverType Type{get;set;}
        public System.Collections.Generic.List<AW.Waypoint> Waypoints{get; set;}

        #endregion

        #region IChanged<Mover> Members

        public event ChangedEventDelegate<Mover> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}