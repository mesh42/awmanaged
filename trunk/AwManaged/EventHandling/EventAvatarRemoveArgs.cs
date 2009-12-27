using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    public sealed class EventAvatarRemoveArgs : IEventAvatarRemoveArgs<Avatar>
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarRemoveArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}