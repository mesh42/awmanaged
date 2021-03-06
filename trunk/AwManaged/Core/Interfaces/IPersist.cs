﻿/* **********************************************************************************
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
using Db4objects.Db4o;

namespace AwManaged.Core.Interfaces
{
    public interface IPersist
    {
        Guid Id { get; }
        void Persist(IObjectContainer db);
    }
}
