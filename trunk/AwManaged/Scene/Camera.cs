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
using AwManaged.Scene.Interfaces;

namespace AwManaged.Scene
{
    public sealed class Camera : MarshalIndefinite, ICamera<Camera>
    {
        #region ICloneableT<Camera> Members

        public Camera Clone()
        {
            return (Camera) MemberwiseClone();
        }

        #endregion

        #region ICamera<Camera> Members

        public AW.CameraFlags Flags { get; set;}
        public string Name { get;set;}
        public float Zoom {get;set;}

        #endregion


        #region IChanged<Camera> Members

        public event ChangedEventDelegate<Camera> OnChanged;

        public bool IsChanged { get; internal set; }

        #endregion
    }
}