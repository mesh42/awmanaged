namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The type specifies the type of light source, which can be either "point" or "spot". "Point" light sources shine equally in all directions, and are the default if no type is specified. "Spot" light sources shine a "cone" of light in a particular direction.
    /// </summary>
    public enum LightType
    {
        /// <summary>
        /// "Point" light sources shine equally in all directions, and are the default if no type is specified.
        /// </summary>
        Point,
        /// <summary>
        /// "Spot" light sources shine a "cone" of light in a particular direction.
        /// </summary>
        Spot
    }
}