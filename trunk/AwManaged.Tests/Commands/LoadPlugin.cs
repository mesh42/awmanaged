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
using System.Linq;
using AwManaged.Core.Commanding;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.ActionInterpreter.Interface;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Linq;

namespace AwManaged.Tests.Commands
{
    public class PersistedPlugin
    {
        [Indexed] private readonly string _name;

        public PersistedPlugin(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }

    [CCGroupBinding(new []{typeof(Load),typeof(Unload)})]
    public class LoadPlugin : IActionCommand, IActionCommandName, ICommandExecute, INeedBotEngineInstance<BotEngine>
    {
        private string _name;
        private bool _isPersistent;

        public LoadPlugin()
        {
            
        }

        public LoadPlugin(string name, bool isPersistent)
        {
            _name = name;
            _isPersistent = isPersistent;
        }

        [CCItemBinding("persist", CommandInterpretType.FlagSlash)]
        public bool IsPersistent
        {
            get { return _isPersistent; }
            set { _isPersistent = value; }
        }

        public List<ICommandGroup> Interpreted
        {
            get; set;
        }

        [CCItemBinding(CommandInterpretType.SingleArgument)]
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
            string persistence = string.Empty;
            if (Interpreted[0].GetType() == typeof(Load))
            {
                BotEngine.LocalBotPluginServicesManager.AddService(Name);
                BotEngine.LocalBotPluginServicesManager.StartService(Name);

                if (IsPersistent)
                {
                    // persist thie command object if it does not exist.
                    var q = from PersistedPlugin p in BotEngine.Storage.Db where p.Name == Name select p;
                    if (q.Count() == 0)
                    {
                        BotEngine.Storage.Db.Store(new PersistedPlugin(Name));
                        BotEngine.Storage.Db.Commit();
                        persistence = " Persisted.";
                    }
                }

                return new CommandExecutionResult() { DisplayMessage = string.Format("Plugin '{0}' loaded.{1}", Name, persistence) };
            }
            if (Interpreted[0].GetType() == typeof(Unload))
            {
                BotEngine.LocalBotPluginServicesManager.StopService(Name);
                BotEngine.LocalBotPluginServicesManager.RemoveService(Name);
                if (IsPersistent)
                {
                    // unpersist thie command object if it exists.
                    var q = from PersistedPlugin p in BotEngine.Storage.Db where p.Name == Name select p;
                    if (q.Count() == 1)
                    {
                        var item = q.Single();
                        BotEngine.Storage.Db.Delete(item);
                        BotEngine.Storage.Db.Commit();
                        persistence = " Persisted.";

                    }
                }
                return new CommandExecutionResult() { DisplayMessage = string.Format("Plugin '{0}' unloaded.{1}", Name, persistence) };
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
