using System.Collections.Generic;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Tests.Commands
{
    public sealed class Unload : IActionTrigger
    {
        #region ITrigger Members

        public string LiteralAction
        {
            get { return "unload"; }
        }

        #endregion

        #region IActionTrigger Members

        public string LiteralCommands
        {
            get;
            set;
        }

        public List<IActionCommand> Commands
        {
            get;
            set;
        }

        #endregion

        public string LiteralPart { get; set; }

    }
}
