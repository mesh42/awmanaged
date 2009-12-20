using System;
using AwManaged;
using AwManaged.Configuration.Interfaces;
using AwManaged.Math;
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