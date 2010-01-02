namespace AwManaged.Scene.ActionInterpreter.Interface
{
    public interface IActionInterpreter<TActionCommand>
    {
        /// <summary>
        /// Creates the action command object from string.
        /// </summary>
        TActionCommand FromString(string action);
        /// <summary>
        /// converts the action command object to string
        /// </summary>
        /// <param name="?"></param>
        string ToString();
    }
}
