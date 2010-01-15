using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Scene.ActionInterpreter
{
    public sealed class ACUnknown : IActionCommand
    {
        public ACUnknown(){}

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "N/A"; } 
        }

        #endregion

        #region ILiteralAction Members

        public string LiteralPart
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
