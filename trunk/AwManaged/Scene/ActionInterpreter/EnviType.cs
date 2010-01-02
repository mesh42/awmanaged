namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The type argument is an integer value that one of the value shown below, where i.e. type 0 will render a the environment from the model's view at it's bounding box center point considering the reflection angle from the main camera's current position; while type 1 will do the same plus mirroring the rendered image.
    /// </summary>
    public enum EnviType
    {
        /// <summary>
        /// 0 ... use bbox center, dynamic reflection angle
        /// </summary>
        BBox_Dynamic_Reflection_Angle = 0,
        /// <summary>
        /// 1 ... use bbox center, dynamic reflection angle, mirror image
        /// </summary>
        BBox_Dynamic_Reflection_Angle_Mirror_Image = 1,
        /// <summary>
        /// 2 ... use bbox center, fixed reflection angle
        /// </summary>
        BBox_Fixed_Reflection_Angle =2,
        /// <summary>
        /// 3 ... use bbox center, fixed reflection angle, mirror image
        /// </summary>
        BBox_Fixed_Reflection_Angle_Mirror_Image = 3,

        /// <summary>
        /// 10 ... use model center, dynamic reflection angle
        /// </summary>
        Model_Dynamic_Reflection_Angle = 10,
        /// <summary>
        /// 11 ... use model center, dynamic reflection angle, mirror image
        /// </summary>
        Model_Dynamic_Reflection_Angle_Mirror_Image = 11,
        /// <summary>
        /// 12 ... use model center, fixed reflection angle
        /// </summary>
        Model_Fixed_Reflection_Angle = 12,
        /// <summary>
        /// 13 ... use model center, fixed reflection angle, mirror image
        /// </summary>
        Model_Fixed_Refelction_Angle_Mirror_Image = 13
    }
}