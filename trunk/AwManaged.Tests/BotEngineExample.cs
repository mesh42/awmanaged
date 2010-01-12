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
using System.IO;
using System.Linq;
using System.Reflection;
using AwManaged.ConsoleServices;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Reflection;
using AwManaged.EventHandling;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Math;
using AwManaged.Scene;
using AwManaged.Scene.ActionInterpreter.Interface;
using AwManaged.Tests.Commands.Attributes;
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

        private bool _isVrtPrompt = true;

        public string Prompt()
        {
            if (_isVrtPrompt)
                return "[VRT " + VrtTime().ToShortTimeString() + " " + LoginConfiguration.Connection.World + ">: ";
            return "[" + DateTime.Now.ToShortTimeString() + " " + LoginConfiguration.Connection.World + ">: ";
        }

        private ChatType _chatType;
        private Avatar _whisperTo;

        public string ChatPrompt()
        {
            switch (_chatType)
            {
                case ChatType.Normal:
                    return "[Send>: ";
                    break;
                case ChatType.Whisper:
                    return "[Whisper (" + _whisperTo.Name + ")>: ";
                default:
                    throw new Exception("Unsupported chat mode.");
            }
        }

        public void ProcessChatMessage(string message)
        {
            if (message.StartsWith("/"))
            {
                var cmd = new CommandLine(message);
                if (cmd.Flags.Count == 0)
                {
                    ConsoleHelpers.WriteLine(ConsoleColor.Red, "Error: unknown flag.");
                    ConsoleHelpers.ReadLine();
                    return;
                }
                switch (cmd.Flags[0].Name.Value)
                {
                    case "h" : case "help" :
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "Supported chat mode commands:");
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "/w <name>  : enter whisper mode with the specified citizen name ");
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "/s         : enter send mode (public chat)");
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "/l         : List available citizen names.");
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "/x         : Exit chat mode.");
                        break;
                    case "l": case "list" :
                        string text = string.Empty;
                        for (int i = 0; i < 20;i++ )
                            foreach (var av in SceneNodes.Avatars)
                            {
                                text += av.Name.PadRight(12);
                            }
                        ConsoleHelpers.WriteLine(ConsoleColor.Green,text);
                        break;
                    case "x": case "exit":
                        _chatType = ChatType.Normal;
                        base.ChatEvent -= ChatMode_ChatEvent;
                        ConsoleHelpers.ParseCommandLine = ProcessCommandLine;
                        ConsoleHelpers.GetPromptTarget = Prompt;
                        ConsoleHelpers.WriteLine(ConsoleColor.DarkGreen, "Exiting chat mode.");
                        break;
                    case "w": case "whisper":
                        if (cmd.Arguments.Count != 1)
                        {
                            ConsoleHelpers.WriteLine(ConsoleColor.Red,"Error: The /w (whisper command needs 1 argument <citizen name>");
                            ConsoleHelpers.ReadLine();
                            return;
                        }
                        var avatar = SceneNodes.Avatars.Find(p => p.Name.ToLower() == cmd.Arguments[0].Value.Value.ToLower());
                        if (avatar == null)
                        {
                            ConsoleHelpers.WriteLine(ConsoleColor.Red, string.Format("Error: No such avatar to whisper to <{0}>", cmd.Arguments[0].Value.Value));
                            ConsoleHelpers.ReadLine();
                            return;
                        }
                        _chatType = ChatType.Whisper;
                        _whisperTo = avatar;
                        break;
                    case "s": case "send":
                        _chatType = ChatType.Normal;
                        break;
                    default:
                        ConsoleHelpers.WriteLine(ConsoleColor.Red, "Error: unknown flag.");
                        break;
                }
            }
            else
            {
                switch (_chatType)
                {
                    case ChatType.Whisper:
                        Whisper(_whisperTo,message);
                        break;
                    case ChatType.Normal:
                        Say(message);
                        break;
                }
                
            }
            ConsoleHelpers.ReadLine();
        }

        public void ProcessCommandLine(string commandLine)
        {
            var ret = _commandInterpeter.Interpret(commandLine);
            bool isInterpreted = false;
            if (ret !=null) // use the object oriented commandline interpreter.
            {
                foreach (var item in ret)
                {
                    foreach (var cmd in item.Commands)
                    {
                        if (ReflectionHelpers.HasInterface(cmd, typeof(ICommandExecute)))
                        {
                            if (ReflectionHelpers.HasInterface(cmd, typeof(INeedBotEngineInstance<BotEngine>)))
                                ((INeedBotEngineInstance<BotEngine>)cmd).BotEngine = this;
                            ICommandExecutionResult result = ((ICommandExecute)cmd).ExecuteCommand();
                            ConsoleHelpers.WriteLine(result.DisplayMessage);
                            isInterpreted = true;
                        }
                    }
                }
                if (isInterpreted)
                {
                    ConsoleHelpers.ReadLine();
                    return;
                }
            }

            switch (commandLine.ToLower())
            {
                case "help":
                    string text = string.Empty;
                    ConsoleHelpers.WriteLine(ConsoleColor.DarkYellow,"Supported Command groups. Type command group <command name> to get specific information about a command.");
                    foreach (var item in _commandInterpeter.Cache.TriggerInterpreters)
                    {
                        text += item.LiteralAction.PadRight(12);
                    }
                    ConsoleHelpers.WriteLine(ConsoleColor.Green,text);
                    break;
                case "list plugins":
                    foreach (var type in BotLocalPlugin.Discover(new DirectoryInfo(Directory.GetCurrentDirectory())))
                    {
                        ConsoleHelpers.WriteLine(ConsoleColor.Green,string.Format("{0} :{1}",type.PluginInfo.TechnicalName.PadRight(24), type.PluginInfo.Description));
                    }
                    break;
                case "list services":
                    foreach (var item in base.ServicesManager.List())
                    {
                        string status;
                        status = item.IsRunning ? "Running" : "Stopped";
                        ConsoleHelpers.WriteLine(ConsoleColor.Green,string.Format("{0} [{1}]",item.TechnicalName.PadRight(50),status));
                    }
                    break;
                case "down":
                    Environment.Exit(0);
                    break;
                case "vrt time": case "vt":
                    _isVrtPrompt = true;
                    break;
                case "local time": case "lt":
                    _isVrtPrompt = false;
                    break;
                case "chat mode": case "cm":
                    ConsoleHelpers.ParseCommandLine = ProcessChatMessage;
                    ConsoleHelpers.GetPromptTarget = ChatPrompt;
                    _chatType = ChatType.Normal;
                    base.ChatEvent += new ChatEventDelegate(ChatMode_ChatEvent);
                    break;
                default:
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleHelpers.WriteLine("?Syntax Error");
                    Console.ForegroundColor = oldColor;
                    break;
            }
            ConsoleHelpers.ReadLine();
        }

        void ChatMode_ChatEvent(BotEngine sender, EventChatArgs e)
        {
            switch (e.ChatType)
            {
                case ChatType.Whisper:
                    ConsoleHelpers.WriteLine(ConsoleColor.DarkYellow, string.Format("[{0} whispers>: {1}", e.Avatar.Name, e.Message));
                    break;
                case ChatType.Normal:
                    ConsoleHelpers.WriteLine(ConsoleColor.Gray, string.Format("[{0}>: {1}", e.Avatar.Name, e.Message));
                    break;
            }
        }

        private GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute,CCItemBindingAttribute> _commandInterpeter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotEngineExample"/> class.
        /// 
        /// You will have to have a valid app.Config settings file, please read documentation on codeplex.
        /// </summary>
        public BotEngineExample()
        {
            _commandInterpeter = new GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute, CCItemBindingAttribute>(Assembly.GetAssembly(typeof(CCEnumBindingAttribute)))
                                     {TechnicalName = "Server Console interpeter"};
            ServicesManager.AddService(_commandInterpeter);
            ServicesManager.StartService(_commandInterpeter.TechnicalName);

            ConsoleHelpers.ParseCommandLine = ProcessCommandLine;
            Console.Title = string.Format("Managed Bot Engine Server {0}\r\n", base.Version());
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();

            ServicesManager.OnServiceStarted += new AwManaged.Core.EventHandling.ServiceStartedelegate(ServicesManager_OnServiceStarted);
            BotEventSlaveStarted += HandleBotEventSlaveStarted;
            BotEventLoggedIn += HandleBotEventLoggedIn;
            BotEventEntersWorld += HandleBotEventEntersWorld;

            ConsoleHelpers.WriteLine(ConsoleColor.Yellow,string.Format("Managed Bot Engine Server {0}\r\nCopyright (C)2009-2010 TCPX\r\n",base.Version()) );
            ConsoleHelpers.WriteLine(ConsoleColor.White, "Starting services");
            ConsoleHelpers.ReadLine();
            _sw1.Start();
            // ensure creation of remote client test account for the AwManaged.RemotingTests console application.
            Start();
        }

        void HandleBotEventSlaveStarted(BotEngine sender, EventBotSlaveStartedArgs e)
        {
            ConsoleHelpers.WriteLine(ConsoleColor.Green,string.Format("Bot slave node {0} started under name [{1}] in world {2}.",e.Node,e.ConnectionProperties.LoginName,e.ConnectionProperties.World));
        }

        void ServicesManager_OnServiceStarted(IServicesManager sender, AwManaged.Core.EventHandling.ServiceStartedArgs e)
        {
            ConsoleHelpers.WriteLine(string.Format("Service {0} started. ({1}).",e.Service.TechnicalName,e.Service.DisplayName));
        }

        /// <summary>
        /// Handles the object event click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleObjectEventClick(BotEngine sender, EventObjectClickArgs e)
        {
            using (var db = sender.Storage.Clone())
            {
                // store the number of clicks on this object in the storage provider.
                ModelClickStatistics stat;
                var query = from ModelClickStatistics p in db where p.ModelId == e.Model.Id select p;
                if (query.Count() == 0)
                    stat = new ModelClickStatistics() {Clicks = 0, ModelId = e.Model.Id};
                else
                    stat = query.Single();
                lock (this)
                {
                    stat.Clicks++;
                    db.Store(stat);
                    db.Commit();

                    ConsoleHelpers.WriteLine(string.Format("object {0} with id {1} clicked by {2}. Total clicks {3}.",
                                                    e.Model.ModelName, e.Model.Id, e.Avatar.Name, stat.Clicks));
                }
            }
        }
        /// <summary>
        /// Handles the object event remove.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleObjectEventRemove(BotEngine sender, EventObjectRemoveArgs e)
        {
            ConsoleHelpers.WriteLine(string.Format("object {0} with id {1} removed.", e.Model.ModelName, e.Model.Id));
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
            ConsoleHelpers.WriteLine(string.Format("object {0} with id {1} added.", e.Model.ModelName, e.Model.Id));
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
            //        ConsoleHelpers.WriteLine(string.Format("Changed model {0}.",model.Id));
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
            ConsoleHelpers.WriteLine(string.Format("Avatar {0} with session {1} has been removed.",e.Avatar.Name,e.Avatar.Session));
            // the rest is now running in the gbot plugin.
        }
        /// <summary>
        /// Handles the avatar event add.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void HandleAvatarEventAdd(BotEngine sender, EventAvatarAddArgs e)
        {
            ConsoleHelpers.WriteLine(String.Format("Avatar {0} with session {1} has been added.", e.Avatar.Name, e.Avatar.Session));
            // the rest is now running in the gbot plugin.
        }
        /// <summary>
        /// Handles the object event scan completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AwManaged.EventHandling.BotEngine.EventObjectScanCompletedEventArgs"/> instance containing the event data.</param>
        void HandleObjectEventScanCompleted(BotEngine sender, EventObjectScanCompletedEventArgs e)
        {
            _sw1.Stop();
            ConsoleHelpers.WriteLine(string.Format("Found {0} objects in {1} ms.", e.SceneNodes.Models.Count,_sw1.ElapsedMilliseconds));
            _sw1.Reset();
            // start receiving object events.
            sender.ObjectEventAdd += HandleObjectEventAdd;
            sender.ObjectEventRemove += HandleObjectEventRemove;
            sender.ObjectEventClick += HandleObjectEventClick;
            sender.ObjectEventChange += HandleObjectEventChange;
            // check how many zone's there are in the world
            ConsoleHelpers.WriteLine(string.Format("Found {0} zones.", e.SceneNodes.Zones.Count));
            ConsoleHelpers.WriteLine(string.Format("Found {0} cameras.", e.SceneNodes.Cameras.Count));
            ConsoleHelpers.WriteLine(string.Format("Found {0} movers.", e.SceneNodes.Movers.Count));
            ConsoleHelpers.WriteLine(string.Format("Found {0} particle emitters.", e.SceneNodes.Particles.Count));
            //string actionContains = "light";
            //ConsoleHelpers.WriteLine(string.Format("object query test, testing object actions which contain {0}",actionContains));
            //var b = from Model p in e.SceneNodes.Models where p.Action.Contains(actionContains) select p;
            //ConsoleHelpers.WriteLine(string.Format("found {0} objects which contain action {1}",b.Count(), actionContains));

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
                                                Position = new Vector3((i * 120) + (-h * 1600), j * 120, 1000)
                                            });
                    }
            sender.AddObjects(transaction);

#endif

#if BACKUP

            ConsoleHelpers.Write("Performing main backup...");
            _sw1.Start();
            var db = sender.Storage.Db;
            //var main_backup = from SceneNodes.SceneNodes p in db select p;
            //if (main_backup.Count() == 1)
            //    db.Delete(main_backup.Single());
            sender.Storage.Db.Store(e.SceneNodes);
            db.Commit();
            ConsoleHelpers.WriteLine("Done!" + _sw1.ElapsedMilliseconds + " ms.");
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
            ConsoleHelpers.WriteLine(string.Format("Interpreted {0} object actions in {1} ms.", triggers.Count, _sw1.ElapsedMilliseconds));

#endif

        }

        void transaction_OnTransactionCompleted(BotEngine sender, EventTransactionCompletedArgs e)
        {
            ConsoleHelpers.WriteLine(string.Format("Transaction {0} committed {1} objects, and completed in {2} ms.",
                e.Transaction.TransactionId,e.Transaction.Commits,e.Transaction.elapsedMs));

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
                            Position = new Vector3((i * 120) + (-h * 1600), j * 120, 2000)
                        });
                    }
            sender.AddObjects(transaction);
        }

        void transaction_OnTransactionCompleted2(BotEngine sender, EventTransactionCompletedArgs e)
        {
            ConsoleHelpers.WriteLine(string.Format("Transaction {0} committed {1} objects, and completed in {2} ms.",
                e.Transaction.TransactionId, e.Transaction.Commits, e.Transaction.elapsedMs));

            foreach (var model in sender.SceneNodes.Models.FindAll(p => p.Action.Contains("@key=unittest")))
            {
                sender.DeleteObject(model);
            }
        }

        static void HandleObjectEventChange(BotEngine sender, EventObjectChangeArgs e)
        {
            ConsoleHelpers.WriteLine(string.Format("object {0} with id {1} changed", new object[]{e.Model.ModelName, e.Model.Id}));
        }

        /// <summary>
        /// Handles the bot event logged in.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void HandleBotEventLoggedIn(BotEngine sender, EventBotLoggedInArgs e)
        {
            ConsoleHelpers.GetPromptTarget = Prompt; // we can start the prompt.

            _sw1.Stop();
            ConsoleHelpers.WriteLine(string.Format("Bot [{0}] logged into the {1} universe server in {2} ms.",
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

            ConsoleHelpers.WriteLine(string.Format("[{0}] Entered world {1} in {2} ms.",e.ConnectionProperties.LoginName,e.ConnectionProperties.World,_sw1.ElapsedMilliseconds));
            _sw1.Reset();
            sender.ObjectEventScanCompleted += HandleObjectEventScanCompleted;
            ConsoleHelpers.Write("Scanning objects...");
            _sw1.Start();
            sender.ScanObjects();
        }
    }
}