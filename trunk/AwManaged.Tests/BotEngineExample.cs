/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Math;
using AwManaged.Scene;
using AwManaged.Scene.ActionInterpreter.Interface;
using Db4objects.Db4o.Linq;

namespace AwManaged.Tests
{
    /// <summary>
    /// Managed Bot Engine Example for testing purposes. Currently only supports V3 objects. Expect updates on V4Object interaction soon.
    /// This bot logs in to a designated world, then sets to receive avatar events and performs a world object scan.
    /// after the scan the bot will make a backup of the world and receive upates on objects.
    /// 
    /// The bot will also transport users that enter the world, and will greet them.
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
    public class BotEngineExample : BotEngine, IHaveToCleanUpMyShit
    {
        /// <summary>
        /// Stopwatch for performance monitoring.
        /// </summary>
        Stopwatch _sw1 = new Stopwatch();
        /// <summary>
        /// Stopwatch for performance monitoring.
        /// </summary>
        Stopwatch _sw2 = new Stopwatch();

        private void WriteLine(ConsoleColor color, string text )
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BotEngineExample"/> class.
        /// 
        /// You will have to have a valid app.Config settings file, please read documentation on codeplex.
        /// </summary>
        public BotEngineExample()
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 48;
            Console.Title = string.Format("Managed Bot Egine Server {0}\r\n", base.Version());
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();

            ServicesManager.OnServiceStarted += new AwManaged.Core.EventHandling.ServiceStartedelegate(ServicesManager_OnServiceStarted);
            BotEventSlaveStarted += new BotEventSlaveStarted(HandleBotEventSlaveStarted);
            BotEventLoggedIn += HandleBotEventLoggedIn;
            BotEventEntersWorld += HandleBotEventEntersWorld;

            WriteLine(ConsoleColor.Yellow,string.Format("Managed Bot Egine Server {0}\r\n",base.Version()) );
            WriteLine(ConsoleColor.White, "Starting services");
            _sw1.Start();
            // ensure creation of remote client test account for the AwManaged.RemotingTests console application.
            Start();
        }

        void HandleBotEventSlaveStarted(BotEngine sender, EventBotSlaveStartedArgs e)
        {
            WriteLine(ConsoleColor.Green,string.Format("Bot slave node {0} started under name [{1}] in world {2}.",e.Node,e.ConnectionProperties.LoginName,e.ConnectionProperties.World));
        }

        void ServicesManager_OnServiceStarted(IServicesManager sender, AwManaged.Core.EventHandling.ServiceStartedArgs e)
        {
            Console.WriteLine("Service {0} started. ({1}).",e.Service.TechnicalName,e.Service.DisplayName);
        }

        /// <summary>
        /// Handles the object event click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleObjectEventClick(BotEngine sender, EventObjectClickArgs e)
        {
            var dep = new DependendObject<Avatar,Model>(e.Avatar, e.Model);
            var db = sender.Storage.Db;
            // store the number of clicks on this object in the storage provider.
            ModelClickStatistics stat;
            var query = from ModelClickStatistics p in db where p.ModelId == e.Model.Id select p;
            if (query.Count() == 0)
                stat = new ModelClickStatistics() {Clicks = 0, ModelId = e.Model.Id};
            else
                stat = query.Single();
            stat.Clicks++;
            db.Store(stat);
            db.Commit();
            Console.WriteLine(string.Format("object {0} with id {1} clicked by {2}. Total clicks {3}.",e.Model.ModelName, e.Model.Id, e.Avatar.Name, stat.Clicks));
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

        Random r = new Random();

        /// <summary>
        /// Handles the object event add.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleObjectEventAdd(BotEngine sender, EventObjectAddArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format("object {0} with id {1} added.", e.Model.ModelName, e.Model.Id));
            //Console.ForegroundColor = ConsoleColor.White;

            if (e.Model.Action.Contains("@change"))
            {
                string htmlColor = ColorTranslator.ToHtml(Color.FromArgb(255, r.Next(255), r.Next(255), (byte) r.Next(255))).Substring(1);
                var model = e.Model;
                model.Action = "create color " + htmlColor  + ";@change";
                model.Description = DateTime.Now.ToLongTimeString();
                sender.ChangeObject(model);
            }

            //// perform some unit tests, these are invoked by the object description.
            //switch (e.Model.Action)
            //{
            //    case "@remove" :
            //        sender.DeleteObject(e.Model);
            //        break;
            //    case "@change" :
            //        var model = e.Model;
            //        model.Description = DateTime.Now.ToLongTimeString();
            //        sender.ChangeObject(model);
            //        Console.WriteLine(string.Format("Changed model {0}.",model.Id));
            //        break;
            //}
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
        /// <summary>
        /// Handles the object event scan completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AwManaged.EventHandling.BotEngine.EventObjectScanCompletedEventArgs"/> instance containing the event data.</param>
        void HandleObjectEventScanCompleted(BotEngine sender, EventObjectScanCompletedEventArgs e)
        {
            _sw1.Stop();
            Console.WriteLine(string.Format("Found {0} objects in {1} ms.", e.SceneNodes.Models.Count,_sw1.ElapsedMilliseconds));
            _sw1.Reset();
            // start receiving object events.
            sender.ObjectEventAdd += HandleObjectEventAdd;
            sender.ObjectEventRemove += HandleObjectEventRemove;
            sender.ObjectEventClick += HandleObjectEventClick;
            sender.ObjectEventChange += HandleObjectEventChange;
            // check how many zone's there are in the world
            Console.WriteLine(string.Format("Found {0} zones.", e.SceneNodes.Zones.Count));
            Console.WriteLine(string.Format("Found {0} cameras.", e.SceneNodes.Cameras.Count));
            Console.WriteLine(string.Format("Found {0} movers.", e.SceneNodes.Movers.Count));
            Console.WriteLine(string.Format("Found {0} particle emitters.", e.SceneNodes.Particles.Count));
            string actionContains = "light";
            Console.WriteLine(string.Format("object query test, testing object actions which contain {0}",actionContains));
            var b = from Model p in e.SceneNodes.Models where p.Action.Contains(actionContains) select p;
            Console.WriteLine(string.Format("found {0} objects which contain action {1}",b.Count(), actionContains));

            //sender.AddObject(new Model(){ModelName="bzmb0.rwx",Position=new Vector3(0,0,10000)});


#if OBJECT_ADD
            // remove all objects which are part of the unit test.
            foreach (var model in e.SceneNodes.Models.FindAll(p => p.Action.Contains("@key=unittest")))
            {
                sender.DeleteObject(model);
            }
            var transaction = new SimpleTransaction<Model>(sender);
            transaction.OnTransactionCompleted += transaction_OnTransactionCompleted;
            for (int h = 0; h < 1; h++)
                for (int j = 0; j < 15; j++)
                    for (int i = 0; i < 15; i++)
                    {
                        transaction.Add(new Model()
                                            {
                                                ModelName = "bzmb0.rwx",
                                                Action = "@key=unittest",
                                                Description = "Bot created object teet",
                                                Position = new Vector3((i * 120) + (-h * 1600), j * 120, 5000)
                                            });
                    }
            sender.AddObjects(transaction);

#endif

#if BACKUP

            Console.Write("Performing main backup...");
            _sw1.Start();
            var db = sender.Storage.Db;
            //var main_backup = from SceneNodes.SceneNodes p in db select p;
            //if (main_backup.Count() == 1)
            //    db.Delete(main_backup.Single());
            sender.Storage.Db.Store(e.SceneNodes);
            db.Commit();
            Console.WriteLine("Done!" + _sw1.ElapsedMilliseconds + " ms.");
#endif

#if ACTION_INTERPRETER

            List<IEnumerable<IActionTrigger>> triggers = new List<IEnumerable<IActionTrigger>>();

            _sw1.Reset();
            _sw1.Start();
            foreach (var model in e.SceneNodes.Models)
            {
                var m = Interpret(model.Action);
                if (m != null)
                {
                    triggers.Add(Interpret(model.Action));
                }
            }
            Console.WriteLine(string.Format("Interpreted {0} object actions in {1} ms.", triggers.Count, _sw1.ElapsedMilliseconds));

#endif

        }

        void transaction_OnTransactionCompleted(BotEngine sender, EventTransactionCompletedArgs e)
        {
            Console.WriteLine("Transaction {0} committed {1} objects, and completed in {2} ms.",
                e.Transaction.TransactionId,e.Transaction.Commits,e.Transaction.elapsedMs);

            foreach (var model in sender.SceneNodes.Models.FindAll(p => p.Action.Contains("@key=unittest")))
            {
                sender.DeleteObject(model);
            }

            // create a second transaction
            var transaction = new SimpleTransaction<Model>(sender);
            transaction.OnTransactionCompleted += transaction_OnTransactionCompleted2;
            for (int h = 0; h < 1; h++)
                for (int j = 0; j < 15; j++)
                    for (int i = 0; i < 15; i++)
                    {
                        transaction.Add(new Model()
                        {
                            ModelName = "bzmb0.rwx",
                            Action = "@key=unittest",
                            Description = "Bot created object teet",
                            Position = new Vector3((i * 120) + (-h * 1600), j * 120, 10000)
                        });
                    }
            sender.AddObjects(transaction);
        }

        void transaction_OnTransactionCompleted2(BotEngine sender, EventTransactionCompletedArgs e)
        {
            Console.WriteLine("Transaction {0} committed {1} objects, and completed in {2} ms.",
                e.Transaction.TransactionId, e.Transaction.Commits, e.Transaction.elapsedMs);

            foreach (var model in sender.SceneNodes.Models.FindAll(p => p.Action.Contains("@key=unittest")))
            {
                sender.DeleteObject(model);
            }
        }

        static void HandleObjectEventChange(BotEngine sender, EventObjectChangeArgs e)
        {
            Console.WriteLine(string.Format("object {0} with id {1} changed", new object[]{e.Model.ModelName, e.Model.Id}));
        }

        /// <summary>
        /// Handles the bot event logged in.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleBotEventLoggedIn(BotEngine sender, EventBotLoggedInArgs e)
        {
            _sw1.Stop();
            Console.WriteLine(string.Format("Bot [{0}] logged into the {1} universe server in {2} ms.",
                e.ConnectionProperties.LoginName, e.ConnectionProperties.Domain,_sw1.ElapsedMilliseconds));
            _sw1.Reset();
            _sw1.Start();
        }

        /// <summary>
        /// Handles the bot event enters world.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleBotEventEntersWorld(BotEngine sender, EventBotEntersWorldArgs e)
        {
            _sw1.Stop();
            // start receiving avatar events.
            sender.AvatarEventAdd += HandleAvatarEventAdd;
            sender.AvatarEventRemove += HandleAvatarEventRemove;

            Console.WriteLine(string.Format("[{0}] Entered world {1} in {2} ms.",e.ConnectionProperties.LoginName,e.ConnectionProperties.World,_sw1.ElapsedMilliseconds));
            _sw1.Reset();
            sender.ObjectEventScanCompleted += HandleObjectEventScanCompleted;
            Console.Write("Scanning objects...");
            _sw1.Start();
            sender.ScanObjects();
        }
    }
}