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
using AwManaged.Core.Commanding;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Tests.Commands
{
    [CCGroupBinding(new []{typeof(Load),typeof(Unload)})]
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

        public List<ICommandGroup> Interpreted
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
                BotEngine.LocalBotPluginServicesManager.AddService(Name);
                BotEngine.LocalBotPluginServicesManager.StartService(Name);
                return new CommandExecutionResult() { DisplayMessage = string.Format("Plugin '{0}' loaded.", Name) };
            }
            if (Interpreted[0].GetType() == typeof(Unload))
            {
                BotEngine.LocalBotPluginServicesManager.StopService(Name);
                BotEngine.LocalBotPluginServicesManager.RemoveService(Name);
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

        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get; set;
        }

        #endregion
    }
}
