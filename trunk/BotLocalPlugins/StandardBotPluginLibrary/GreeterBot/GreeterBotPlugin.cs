using AwManaged;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.Math;
using AwManaged.Scene;

namespace StandardBotPluginLibrary.GreeterBot
{
    [PluginInfo("gbot","This is a simple greeterbot.")]
    public class GreeterBotPlugin :  BotLocalPlugin
    {
        public GreeterBotPlugin(BotEngine bot) : base(bot)
        {
            bot.AvatarEventAdd += bot_AvatarEventAdd;
            bot.AvatarEventRemove += bot_AvatarEventRemove;
        }

        void bot_AvatarEventRemove(BotEngine sender, AwManaged.EventHandling.BotEngine.EventAvatarRemoveArgs e)
        {
            sender.Say(5000, SessionArgumentType.AvatarSessionMustNotExist, e.Avatar, string.Format("{0} has left {1}.", e.Avatar.Name, sender.LoginConfiguration.Connection.World));
        }

        void bot_AvatarEventAdd(BotEngine sender, AwManaged.EventHandling.BotEngine.EventAvatarAddArgs e)
        {
            // don't perform actions when the bot has been logged in shorter than 10 seconds (prevent greeting flood).
            //if (sender.ElapsedMilliseconds > 10000)
            //{
            // transport the avatar to a certain location.
            var teleport = new Vector3(500, 500, 500);
            //sender.Teleport(e.Avatar, 500, 500, 500, 45);
            // send a message that the user has entered the world and has been teleported to a certain location (with a time delay).
            var message = string.Format("Teleported {0} to location {1},{2},{3}",
                                        new[]
                                                {
                                                    e.Avatar.Name, teleport.x.ToString(), teleport.y.ToString(),
                                                    teleport.z.ToString()
                                                });
            sender.Say(5000, SessionArgumentType.AvatarSessionMustExist, e.Avatar,
                       string.Format("{0} enters.", e.Avatar.Name));
            sender.Say(6000, SessionArgumentType.AvatarSessionMustExist, e.Avatar, message);
            //}

        }
    }
}
