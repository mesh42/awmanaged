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
using AwManaged.Core.Services.Interfaces;

namespace AwManaged
{
    public sealed class PluginContext
    {
        public PluginInfoAttribute PluginInfo;
        public Type Type;
    }

    public abstract class BotLocalPlugin : IDisposable, IService
    {
        #region static Reflection Helpers

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

        #endregion

        public BotEngine Bot { get; internal set; }

        private readonly PluginContext _pluginContext;

        protected BotLocalPlugin(BotEngine engineReference)
        {
            Bot = engineReference;
            _pluginContext = GetPluginContext(GetType());
        }

        public virtual void PluginInitialized()
        {
            Bot.Console.WriteLine(string.Format("{0}: Plugin Intialized.", _pluginContext.PluginInfo.TechnicalName));
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            Bot.Console.WriteLine(string.Format("{0}: Plugin Terminated.", _pluginContext.PluginInfo.TechnicalName));
            GC.Collect();
        }

        #endregion

        #region IService Members

        public bool Stop()
        {
            if (!IsRunning)
                throw new Exception("can't stop, the plugin is currently not running.");
            IsRunning = false;
            return true;
        }

        public bool Start()
        {
            if (IsRunning)
                throw new Exception("can't start, the plugin is currently not running.");
            IsRunning = true;
            return true;
        }

        public bool IsRunning
        {
            get; private set;
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get { return _pluginContext.PluginInfo.Description; }
        }

        public Guid IdentifyableId
        {
            get { throw new NotImplementedException(); }
        }

        public string IdentifyableTechnicalName
        {
            get { return _pluginContext.PluginInfo.TechnicalName; }
        }

        #endregion
    }
}
