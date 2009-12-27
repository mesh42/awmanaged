using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.SceneNodes;

namespace AwManaged.EventHandling
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EventAvatarAddArgs : IEventAvatarAddArgs<Avatar>
    {
        public Avatar Avatar { get; private set; }

        public EventAvatarAddArgs(ICloneableT<Avatar> avatar)
        {
            Avatar = avatar.Clone();
        }
    }
}