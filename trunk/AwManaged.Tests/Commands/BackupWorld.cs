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
using System.Diagnostics;
using AwManaged.Core.Commanding;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Core.Interfaces;
using AwManaged.Scene.ActionInterpreter.Interface;
using AwManaged.Storage.BackupProvider;

namespace AwManaged.Tests.Commands
{
    [CCGroupBinding(new[] {typeof(Backup)})]
    [CCHelpDescription("Backup the contents of a world.")]
    public sealed class BackupWorld : IActionCommand, ICommandExecute, INeedBotEngineInstance<BotEngine>
    {
        public BackupRecord Record { get; set; }

        public BackupWorld()
        {
        }

        [CCItemBinding("label",CommandInterpretType.SingleArgument)]
        public string Label
        {
            get; set;
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "world"; }
        }

        public string LiteralPart
        {
            get; set;
        }

        #endregion

        #region ICommandExecute Members

        public ICommandExecutionResult ExecuteCommand()
        {
            try
            {
                var sw1 = new Stopwatch();
                using (var db = BotEngine.Storage.Clone())
                {
                    sw1.Start();
                    db.Store(BotEngine.SceneNodes);
                    db.Commit();
                }
                sw1.Stop();
                var ret = new CommandExecutionResult
                              {
                                  DisplayMessage =
                                      string.Format("World backup completed in {0} ms.", sw1.ElapsedMilliseconds)
                              };
                return ret;
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult() {DisplayMessage = ex.Message};
            }
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
            get;
            set;
        }

        #endregion
    }
}
