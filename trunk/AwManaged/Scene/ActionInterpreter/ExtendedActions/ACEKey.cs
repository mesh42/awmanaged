using AwManaged.Core.Commanding;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Scene.ActionInterpreter.ExtendedActions
{
    /// <summary>
    /// Extended command for Bot Engines, @key indentifies an object by a label, which can be tracked by the bot.
    /// sometimes it is not desired to track an object by name or by object number.
    /// </summary>
    public class ACEKey : IActionCommand
    {
        #region ILiteralAction Members

        public ACEKey()
        {
            
        }

        [ACItemBinding("value", CommandInterpretType.NameValuePairs)]
        public string Value
        {
            get; set;
        }

        public string LiteralAction
        {
            get { return "@key"; }
        }

        public string LiteralPart
        {
            get;set;
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