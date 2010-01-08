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
    public interface IModel<T> : ISceneNode<T>
        where T : MarshalByRefObject
    {
        int Id { get; }
        int Owner { get; set; }
        DateTime Timestamp { get; }
        ObjectType Type { get; }
        string ModelName { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        string Description { get; set; }
        string Action { get; set; }
        string Data { get; set; }
        //int Number { get; }
    }
}