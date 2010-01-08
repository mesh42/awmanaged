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
using System.Collections.Generic;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Core.Reflection
{
    public sealed class ReflectionCache
    {
        public List<IActionTrigger> TriggerInterpreters = ReflectionHelpers.GetInstancesOfInterface<IActionTrigger>(); 
        public List<IActionCommand> CommandInterpreters = ReflectionHelpers.GetInstancesOfInterface<IActionCommand>();
        public ReflectionEnumCache EnumCache = ReflectionHelpers.GetEnums<ACEnumTypeAttribute, ACEnumBindingAttribute>();
        public ReflectionCache()
        {
        }
    }
}
