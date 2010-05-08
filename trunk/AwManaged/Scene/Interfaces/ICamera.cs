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
using AwManaged.Core.Patterns;

namespace AwManaged.Scene.Interfaces
{
    public interface ICamera<T> : ISceneNode<T>
        where T : MarshalIndefinite
    {
        AW.CameraFlags Flags { set; get; }
        string Name { set; get; }
        float Zoom { set; get; }
    }
}