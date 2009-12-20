using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IEventObjectClickArgs
    {
        /// <summary>
        /// The object the user clicked on.
        /// </summary>
        IModel Object { get; set; }
        /// <summary>
        /// The user who clicked the object.
        /// </summary>
        IAvatar Avatar { get; set; }
    }
}