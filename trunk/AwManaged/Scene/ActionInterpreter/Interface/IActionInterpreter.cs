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
namespace AwManaged.Scene.ActionInterpreter.Interface
{
    public interface IActionInterpreter<TActionCommand>
    {
        /// <summary>
        /// Creates the action command object from string.
        /// </summary>
        TActionCommand FromString(string action);
        /// <summary>
        /// converts the action command object to string
        /// </summary>
        /// <param name="?"></param>
        string ToString();
    }
}
