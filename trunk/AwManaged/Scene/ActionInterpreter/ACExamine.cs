namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The examine command marks the object as an object that can be "examined", which means users can hold down the left mouse button on the object and move the mouse to rotate it in three dimensions in order to examine all sides of it without having to move themselves. The object will rotate around it's own object axis. When an object is examinable, the mouse cursor changes to a four-direction arrow when placed over the object in order to indicate that it can be examined with the mouse.
    /// </summary>
    public class ACExamine
    {
        /// <summary>
        /// Single command, no parameters.
        /// </summary>
        public ACExamine()
        {
            
        }
    }
}
