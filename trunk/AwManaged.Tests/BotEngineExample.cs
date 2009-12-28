using System;
using System.Collections.Generic;
using System.Linq;
using AwManaged;
using AwManaged.Configuration.Interfaces;
using AwManaged.EventHandling;
using AwManaged.Math;
using AwManaged.SceneNodes;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;


namespace AwManaged.Tests
{
    /// <summary>
    /// Managed Bot Engine Example for testing purposes. Currently only supports V3 objects. Expect updates on V4Object interaction soon.
    /// This bot logs in to a designated world, then sets to receive avatar events and performs a world object scan.
    /// after the scan the bot will receive upates on objects.
    /// 
    /// The bot will also trnasport users that enter the world, and will greet them.
    /// 
    /// There is a unit test which is evaluated when one or multiple objects are added in the world:
    /// 
    /// As part of this unit test, the bot will interact with objects that have either:
    /// 
    /// @remove or @change in their description field. 
    /// 
    /// The main purpose for this is to unit test massive updates in the world, currently tests are being done with
    /// 40 objects simultaniously with good results.
    /// </summary>
    public class BotEngineExample : BotEngine, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotEngineExample"/> class.
        /// 
        /// You will have to have a valid app.Config settings file, please read documentation on codeplex.
        /// </summary>
        public BotEngineExample()
        {
            BotEventLoggedIn += HandleBotEventLoggedIn;
            BotEventEntersWorld += HandleBotEventEntersWorld;
            Console.WriteLine("AwManaged BotEngine Example");
            Console.WriteLine("Storage server running.");
            try
            {
                Console.WriteLine("Connecting to universe...");
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// Handles the object event click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleObjectEventClick(BotEngine sender, EventObjectClickArgs e)
        {
            var db = sender.Storage.Db;

            // store the number of clicks on this object in the storage provider.
            ModelClickStatistics stat;
            var query = from ModelClickStatistics p in db where p.ModelId == e.Model.Id select p;
            if (query.Count() == 0)
                stat = new ModelClickStatistics() { Clicks = 0, ModelId = e.Model.Id };
            else
                stat = query.Single();
            stat.Clicks++;
            db.Store(stat);
            db.Commit();
            Console.WriteLine(string.Format("object {0} with id {1} clicked by {2}. Total clicks {3}.", e.Model.ModelName, e.Model.Id,e.Avatar.Name,stat.Clicks));
        }

        /// <summary>
        /// Handles the object event remove.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleObjectEventRemove(BotEngine sender, EventObjectRemoveArgs e)
        {
            Console.WriteLine(string.Format("object {0} with id {1} removed.", e.Model.ModelName, e.Model.Id));
        }

        /// <summary>
        /// Handles the object event add.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleObjectEventAdd(BotEngine sender, EventObjectAddArgs e)
        {
            Console.WriteLine(string.Format("object {0} with id {1} added.", e.Model.ModelName,e.Model.Id));

            // perform some unit tests, these are invoked by the objct description.
            switch (e.Model.Description)
            {
                case "@remove" :
                    sender.DeleteObject(e.Model);
                    break;
                case "@change" :
                    e.Model.Description = DateTime.Now.ToLongTimeString();
                    sender.ChangeObject(e.Model);
                    break;
            }
        }

        /// <summary>
        /// Handles the avatar event remove.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleAvatarEventRemove(BotEngine sender, EventAvatarRemoveArgs e)
        {
            Console.WriteLine("Avatar {0} with session {1} has been removed.",e.Avatar.Name,e.Avatar.Session);
            sender.Say(5000, SessionArgumentType.AvatarSessionMustNotExist, e.Avatar, string.Format("{0} has left {1}.", e.Avatar.Name, sender.LoginConfiguration.Connection.World));
        }

        /// <summary>
        /// Handles the avatar event add.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleAvatarEventAdd(BotEngine sender, EventAvatarAddArgs e)
        {
            Console.WriteLine("Avatar {0} with session {1} has been added.", e.Avatar.Name, e.Avatar.Session);
            // transport the avatar to a certain location.
            var teleport = new Vector3(500,500,500);
            sender.Teleport(e.Avatar, 500, 500, 500, 45);
            // send a message that the user has entered the world and has been teleported to a certain location (with a time delay).
            var message = string.Format("Teleported {0} to location {1},{2},{3}",
                          new[] {e.Avatar.Name, teleport.x.ToString(), teleport.y.ToString(), teleport.z.ToString()});
            sender.Say(5000,SessionArgumentType.AvatarSessionMustExist, e.Avatar,string.Format("{0} enters.", e.Avatar.Name));
            sender.Say(6000,SessionArgumentType.AvatarSessionMustExist, e.Avatar, message);
        }

        /// <summary>
        /// Handles the object event scan completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AwManaged.EventHandling.EventObjectScanCompletedEventArgs"/> instance containing the event data.</param>
        static void HandleObjectEventScanCompleted(BotEngine sender, EventObjectScanCompletedEventArgs e)
        {
            Console.WriteLine(string.Format("Found {0} objects.", e.Model.Count));
            // start receiving object events.
            sender.ObjectEventAdd += HandleObjectEventAdd;
            sender.ObjectEventRemove += HandleObjectEventRemove;
            sender.ObjectEventClick += HandleObjectEventClick;
            sender.ObjectEventChange += HandleObjectEventChange;
        }

        static void HandleObjectEventChange(BotEngine sender, EventObjectChangeArgs e)
        {
            Console.WriteLine(
                string.Format("object {0} with id {1} changed. old number {2}, new number {3}", 
                new object[]{e.Model.ModelName, e.Model.Id,e.OldModel.Number,e.Model.Number}));
        }

        /// <summary>
        /// Handles the bot event logged in.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleBotEventLoggedIn(BotEngine sender, IUniverseConnectionProperties e)
        {
            Console.WriteLine(string.Format("Bot [{0}] logged into the {1} universe server on port {2}",e.LoginName, e.Domain,e.Port));
        }

        /// <summary>
        /// Handles the bot event enters world.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleBotEventEntersWorld(BotEngine sender, IUniverseConnectionProperties e)
        {
            // start receiving avatar events.
            sender.AvatarEventAdd += HandleAvatarEventAdd;
            sender.AvatarEventRemove += HandleAvatarEventRemove;

            Console.WriteLine(string.Format("[{0}] Entered world {1}.",e.LoginName,e.World));
            sender.ObjectEventScanCompleted += HandleObjectEventScanCompleted;
            Console.Write("Scanning objects...");
            sender.ScanObjects();
        }
    }
}