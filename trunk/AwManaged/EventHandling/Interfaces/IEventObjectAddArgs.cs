using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectAddArgs
    {
        /// <summary>
        /// The object the user added.
        /// </summary>
        IModel Object { get; set; }
        /// <summary>
        /// The user who added the object.
        /// </summary>
        IAvatar Avatar { get; set; }
    }
}