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
    /// <summary>
    /// Displays help in a generic fashion through reflection.
    /// </summary>
    [CCHelpDescription("Generic help.")]
    public class Help : ICommandGroup, ICommandExecute, INeedBotEngineInstance<BotEngine>
    {
        private string _name;

        public Help()
        {

        }

        public List<ICommandGroup> Interpreted
        {
            get;
            set;
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
            get { return "help"; }
        }

        public string LiteralPart
        {
            get;
            set;
        }

        #endregion

        #region ICommandExecute Members

        public ICommandExecutionResult ExecuteCommand()
        {
            // do some reflection to get appropiate help.


            return new CommandExecutionResult() { DisplayMessage = string.Format("Plugin '{0}' unloaded.", Name) };
        }

        #endregion

        #region INeedBotEngineInstance<BotEngine> Members

        public BotEngine BotEngine
        {
            get;
            set;
        }

        #endregion

        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }

        #endregion

        #region IActionTrigger Members

        public List<IActionCommand> Commands
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion
    }
}
