using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace Bot.Logic.AWManaged
{
    public class EventObjectClickArgs : IEventObjectClickArgs
    {
        /// <summary>
        /// The object the user added.
        /// </summary>
        public IModel Object { get; set; }
        /// <summary>
        /// The user who added the object.
        /// </summary>
        public IAvatar Avatar { get; set; }

        public EventObjectClickArgs(IModel o, IAvatar avatar)
        {
            Object = o;
            Avatar = avatar;
        }
    }
}
