using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes.Interfaces;

namespace AwManaged.EventHandling
{
    public class EventAvatarRemoveArgs : IEventAvatarRemoveArgs
    {
        private IAvatar _avatar;

        public IAvatar Avatar { get { return _avatar;} }

        public EventAvatarRemoveArgs(IAvatar avatar)
        {
            _avatar = avatar;
        }
    }
}