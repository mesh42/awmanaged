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
using System.Linq;
using System.Windows.Forms;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Services;
using AwManaged.Core.Services.Interfaces;
using AwManaged.Core.ServicesManaging;

namespace AwManaged.LocalServices
{
    public class LocalBotPluginServicesManager : BaseServicesManager, INeedBotEngineInstance<BotEngine>
    {
        private List<BotLocalPlugin> _pluginInstances;
        private List<PluginContext> _pluginDiscovery;

        public List<PluginContext> PluginDiscovery { get { return _pluginDiscovery; } }

        #region IService Members

       

        public LocalBotPluginServicesManager(BotEngine botEngine)
        {
            BotEngine = botEngine;
            _pluginDiscovery = BotLocalPlugin.Discover(new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath)));
        }

        public override bool Stop()
        {
            base.Stop();
            foreach(var item in base.List())
            {
                ((BotLocalPlugin)item).Dispose();
            }
            _pluginInstances.Clear();
            return true;
        }

        public override bool Start()
        {
            _pluginInstances = new List<BotLocalPlugin>();
            return base.Start();
        }

        #endregion

        #region IIdentifiable Members

        public override string IdentifyableDisplayName
        {
            get { return "Local bot plugin service."; }
        }

        public override System.Guid IdentifyableId
        {
            get { return new Guid("{B1769AA4-ABEB-4d02-A8E3-467CD9C7B23F}"); }
        }

        public override string IdentifyableTechnicalName
        {
            get { return "localbotpluginsvc";  }
        }

        #endregion

        #region INeedBotEngineInstance<BotEngine> Members

        public BotEngine BotEngine
        {
            get; set;
        }

        #endregion

        #region IServicesManager Members

        public void AddService(string technicalName)
        {
            var contexts = from PluginContext p in _pluginDiscovery where p.PluginInfo.TechnicalName == technicalName select p; // todo: _plugin discovery might need to be refreshed, maybe use a directory whatcher or something.
            if (contexts.Count() == 0)
                throw new Exception(string.Format("Plugin with technical name {0} not found.",technicalName));
            var context = contexts.Single();
            var service = new DummyService() {IdentifyableTechnicalName = context.PluginInfo.TechnicalName};
            base.AddService(service);
        }

        public override IService StartService(string technicalName)
        {
            var contexts = from PluginContext p in _pluginDiscovery where p.PluginInfo.TechnicalName == technicalName select p; // todo: _plugin discovery might need to be refreshed, maybe use a directory whatcher or something.
            if (contexts.Count() == 0)
                throw new Exception(string.Format("Plugin with technical name {0} not found.", technicalName));
            var context = contexts.Single();
            var constructor = context.Type.GetConstructor(new[] { typeof(BotEngine) });
            var baseService = base.StartService(technicalName);
            var plugin = (BotLocalPlugin) constructor.Invoke(new object[] {BotEngine});
            _pluginInstances.Add(plugin);
            plugin.PluginInitialized();
            return baseService;
        }

        public override void StopService(string technicalName)
        {
            base.StopService(technicalName);
            var instances = from BotLocalPlugin p in _pluginInstances where (p.IdentifyableTechnicalName == technicalName) select p;
            var instance = instances.Single();
            _pluginInstances.Remove(instance);
            instance.Dispose();
        }

        public override bool StartServices()
        {
            base.StartServices();
            foreach (var item in base.List())
            {
                var constructor = item.GetType().GetConstructor(new[] { typeof(BotEngine) });
                _pluginInstances.Add((BotLocalPlugin)constructor.Invoke(new object[] { BotEngine }));
            }
            return true;
        }

        public event AwManaged.Core.EventHandling.ServiceStartedelegate OnServiceStarted;

        #endregion
    }
}
