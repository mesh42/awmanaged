using System;
using AwManaged;
using AwManaged.Configuration.Interfaces;
using AwManaged.EventHandling.Interfaces;
using AwManaged.Math;
using AwManaged.SceneNodes;
using AWManaged.Security;
using AwManaged.Interfaces;

namespace AwManaged.Tests
{
    /// <summary>
    /// Managed Bot Engine Example for testing purposes.
    /// </summary>
    public class BotEngineExample : BaseBotEngine, IDisposable
    {
        public BotEngineExample(Authorization authorization, string domain, int port, int loginOwner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
            : base(authorization, domain, port, loginOwner, privilegePassword, loginName, world, position, rotation)
        {
            BotEventLoggedIn += HandleBotEventLoggedIn;
            BotEventEntersWorld += HandleBotEventEntersWorld;
            Console.WriteLine("AwManaged BotEngine Example");
            try
            {
                Console.WriteLine("Connecting to universe...");
                Start();
                ObjectEventScanCompleted += HandleObjectEventScanCompleted;
                Console.Write("Scanning objects...");
                ScanObjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            AvatarEventAdd += HandleAvatarEventAdd;
            AvatarEventRemove += HandleAvatarEventRemove;
            ObjectEventAdd += HandleObjectEventAdd;
        }

        void HandleObjectEventAdd(IBaseBotEngine sender, IEventObjectAddArgs e)
        {
            Console.WriteLine(string.Format("object {0} with id {1} added.", e.Object.ModelName,e.Object.Id));
        }

        void HandleAvatarEventRemove(IBaseBotEngine sender, IEventAvatarRemoveArgs e)
        {
            Say(5000, SessionArgumentType.AvatarSessionMustNotExist, e.Avatar, string.Format("{0} has left {1}.", e.Avatar.Name, sender.LoginConfiguration.World));
        }

        void HandleAvatarEventAdd(IBaseBotEngine sender, IEventAvatarAddArgs e)
        {
            var teleport = new Vector3(500,500,500);
            // transport the avatar to a certain location.
            
            var message = string.Format("Teleported {0} to location {1},{2},{3}",
                          new[] {e.Avatar.Name, teleport.x.ToString(), teleport.y.ToString(), teleport.z.ToString()});
            Say(5000,SessionArgumentType.AvatarSessionMustExist, e.Avatar,string.Format("{0} enters.", e.Avatar.Name));
            Say(10000,SessionArgumentType.AvatarSessionMustExist, e.Avatar, message);
            SetPosition(e.Avatar, 500, 500, 500,45);
        }

        static void HandleObjectEventScanCompleted(IBaseBotEngine sender, IEventObjectScanEventArgs e)
        {
            Console.WriteLine(string.Format("Found {0} objects.", e.Model.Count));
        }

        static void HandleBotEventLoggedIn(IBaseBotEngine sender, ILoginConfiguration e)
        {
            Console.WriteLine(string.Format("Bot [{0}] logged into the {1} universe server on port {2}",e.LoginName, e.Domain,e.Port));
        }

        static void HandleBotEventEntersWorld(IBaseBotEngine sender, ILoginConfiguration e)
        {
            Console.WriteLine(string.Format("[{0}] Entered world {1}.",e.LoginName,e.World));
        }
    }
}