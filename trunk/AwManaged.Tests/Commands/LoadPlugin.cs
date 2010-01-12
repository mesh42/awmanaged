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
using System.IO;
using System.Linq;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;
using AwManaged.Tests.Commands.Attributes;

namespace AwManaged.Tests.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadPlugin : IActionCommand, IActionCommandName, ICommandExecute, INeedBotEngineInstance<BotEngine>
    {
        private string _name;

        public LoadPlugin()
        {
            
        }

        public LoadPlugin(string name)
        {
            _name = name;
        }

        public List<IActionTrigger> Interpreted
        {
            get; set;
        }

        [CCItemBinding("label", CommandInterpretType.SingleArgument)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "plugin"; }
        }

        public string LiteralPart
        {
            get; set;
        }

        #endregion

        #region ICommandExecute Members

        public ICommandExecutionResult ExecuteCommand()
        {
            if (Interpreted[0].GetType() == typeof(Load))
            {
                var pluginContexts = BotLocalPlugin.Discover(new DirectoryInfo(Directory.GetCurrentDirectory()));
                var q = from PluginContext p in pluginContexts where p.PluginInfo.TechnicalName == Name select p;
                if (q.Count() == 0)
                {
                    return new CommandExecutionResult() {DisplayMessage = string.Format("Plugin '{0}' not found.", Name)};
                }
                var context = q.Single();
                var constructor = context.Type.GetConstructor(new[] {typeof (BotEngine)});
                var plugin = constructor.Invoke(new object[]{BotEngine}); // todo: should load this into the botengine's service manager.
                return new CommandExecutionResult() {DisplayMessage = string.Format("Plugin '{0}' loaded.", Name)};
            }
            if (Interpreted[0].GetType() == typeof(Unload))
            {
                return new CommandExecutionResult() {DisplayMessage = string.Format("Plugin '{0}' unloaded.", Name)};
            }
            return new CommandExecutionResult()
                       {
                           DisplayMessage = string.Format("Command '{0}' is not part of command group '{1}'.", LiteralAction, Interpreted[0].LiteralAction)
                       };
        }

        #endregion

        #region INeedBotEngineInstance<BotEngine> Members

        public BotEngine BotEngine
        {
            get; set;
        }

        #endregion
    }
}
