using AwManaged.ScriptServices;
using AwManaged.Interfaces;
using AwManaged.EventHandling.Interfaces;

namespace AwManagedIde.Scripts
{
    public class SimpleScript : BaseBotInteractionScript
    {
        public SimpleScript()
        {
            AvatarEventAdd += new AwManaged.BaseBotEngine.AvatarEventAddDelegate(SimpleScript_AvatarEventAdd);
        }

        void SimpleScript_AvatarEventAdd(IBaseBotEngine sender, IEventAvatarAddArgs e)
        {
        }
    }
}
