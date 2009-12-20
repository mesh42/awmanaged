using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling
{
    public class EventAvatarAddArgs : IEventAvatarAddArgs
    {
        public IAvatar Avatar { get; set; }

        public EventAvatarAddArgs(IAvatar avatar)
        {
            Avatar = avatar;
        }
    }
}