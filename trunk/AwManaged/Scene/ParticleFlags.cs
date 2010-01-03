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
using AwManaged.Core.Interfaces;
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public class ParticleFlags : MarshalByRefObject, IParticleFlags<ParticleFlags> 
    {
        public bool CameraEmit { get; set; }
        public bool DrawInFront { get; set; }
        public bool Gravity { get; set; }
        public bool Interpolate { get; set; }
        public bool LinkToMover { get; set; }
        public bool ZoneCollision { get; set; }
        public bool ZoneExclusive { get; set;}
        
        public ParticleFlags Clone()
        {
            return (ParticleFlags) MemberwiseClone();
        }

        public event ChangedEventDelegate<ParticleFlags> OnChanged;
        public bool IsChanged
        {
            get { throw new NotImplementedException(); }
        }
    }
}
