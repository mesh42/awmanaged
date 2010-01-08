using AwManaged.Scene.ActionInterpreter.Attributes;

namespace AwManaged.Scene.ActionInterpreter.Interface
{
    public interface IActionCommandGlobal
    {
        /// <summary>
        /// The global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.
        /// </summary>
        /// <value><c>true</c> if this instance is global; otherwise, <c>false</c>.</value>
        [ACItemBinding("global", CommandInterpretType.Flag)]
        GlobalType IsGlobal {get;set;}
    }
}
