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

namespace AwManaged.Core.Reflection.Attributes
{
    public sealed class PluginInfoAttribute : Attribute
    {
        public string TechnicalName { get; private set; }
        public string Description { get; private set; }

        public PluginInfoAttribute(string technicalName, string description)
        {
            TechnicalName = technicalName;
            Description = description;
        }
    }
}
