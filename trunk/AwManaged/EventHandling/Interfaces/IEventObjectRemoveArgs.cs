using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectRemoveArgs
    {
        /// <summary>
        /// The object the user removed. (This is a cloned object of the original, as the actual object does not exist anymore with the AW SDK implementation).
        /// </summary>
        IModel Object { get; set; }
        /// <summary>
        /// The user who removed the object.
        /// </summary>
        IAvatar Avatar { get; set; }
    }
}