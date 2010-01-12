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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AwManaged.Core.Reflection;
using AwManaged.Core.Reflection.Attributes;

namespace AwManaged
{
    public sealed class PluginContext
    {
        public PluginInfoAttribute PluginInfo;
        public Type Type;
    }

    public class BotLocalPlugin
    {
        private static PluginContext GetPluginContext(Type type)
        {
            //var asm = Assembly.GetAssembly(type);
            var att = type.GetCustomAttributes(typeof(PluginInfoAttribute), false);
            if (att !=null && att.Length == 1 && type.BaseType == typeof(BotLocalPlugin))
            {
                return new PluginContext() {PluginInfo = (PluginInfoAttribute) att[0], Type = type};
            }
            return null;
        }

        public static List<PluginContext> Discover(DirectoryInfo directory)
        {
            var ret = new List<PluginContext>();
            foreach (var dll in directory.GetFiles("*.dll"))
            {
                try
                {
                    foreach (var type in ReflectionHelpers.GetTypes(Assembly.LoadFile(dll.FullName/*.Replace(".dll", "")*/), typeof (BotLocalPlugin)))
                    {
                        ret.Add(GetPluginContext(type));
                    }
                }
                catch{/*ignore*/}
            }
            return ret;
        }

        public BotEngine Bot { get; internal set; }

        public BotLocalPlugin(BotEngine engineReference)
        {
            Bot = engineReference;
        }
    }
}
