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
using System;
using System.Collections.Generic;
using System.Reflection;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Core.Reflection
{
    public sealed class ReflectionCache<TEnumTypeAttribute, TEnumBindingAttribute>
        where TEnumTypeAttribute : Attribute
        where TEnumBindingAttribute : Attribute, IACLiteralNames
    {
        private readonly Assembly _assembly;
        public List<IActionTrigger> TriggerInterpreters;
        public List<IActionCommand> CommandInterpreters;
        public ReflectionEnumCache EnumCache;
       
        public ReflectionCache(Assembly assembly)
        {
            _assembly = assembly;
            TriggerInterpreters = ReflectionHelpers.GetInstancesOfInterface<IActionTrigger>(assembly);
            CommandInterpreters = ReflectionHelpers.GetInstancesOfInterface<IActionCommand>(assembly);
            EnumCache = ReflectionHelpers.GetEnums<TEnumTypeAttribute, TEnumBindingAttribute>(assembly);
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }
    }
}
