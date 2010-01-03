namespace AwManaged.Scene.ActionInterpreter.Interface
{
    public interface ITrigger
    {
        /// <summary>
        /// Gets the literal command, which is the command before interpretation.
        /// </summary>
        /// <value>The literal command.</value>
        string LiteralCommand { get; }
    }
}
