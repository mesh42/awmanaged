using AwManaged.EventHandling.Interfaces;

namespace AwManaged.EventHandling
{
    public class EventAvatarRemoveArgs : IEventAvatarRemoveArgs
    {
        public int Session { get; set; }

        public EventAvatarRemoveArgs(int session)
        {
            Session = session;
        }
    }
}