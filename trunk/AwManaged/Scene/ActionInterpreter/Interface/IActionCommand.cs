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
using System.Collections.Generic;

namespace AwManaged.Scene.ActionInterpreter.Interface
{
    public interface IActionCommand : ILiteralAction, ICommandGroups
    {
    }

    public interface ICommandGroups
    {
        IList<ICommandGroup> CommandGroups { get; }
    }
}
