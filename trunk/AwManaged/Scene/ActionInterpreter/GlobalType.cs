using AwManaged.Scene.ActionInterpreter.Attributes;

namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// Global type argument/enum.
    /// </summary>
    [ACEnumType]
    public enum GlobalType
    {
        /// <summary>
        /// Unspecified = non global
        /// </summary>
        NonGlobal = 0,
        /// <summary>
        /// Global flag found.
        /// </summary>
        [ACEnumBinding(new[] { "global" })]
        Global = 1,
    }
}
