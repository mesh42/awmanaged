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
using AwManaged;
using AwManaged.Core.Commanding;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Scene;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using StandardBotPluginLibrary.GreeterBot;
using System.Linq;

namespace StandardBotPluginLibrary.AfkBot
{
    /// <summary>
    /// Simple greeter bot. To demonstrate The Local Plugin Provider
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("afkbot", "This is a simple afk bot.")] /* plugin information for the plugin provider */
    public class AfkBotPlugin : BotLocalPlugin
    {
        /// <summary>
        /// A dedicated isntance to the bot engine's application storage database.
        /// </summary>
        private readonly IObjectContainer _db;
        /// <summary>
        /// An in memory status list which keeps track of avatar idle status.
        /// </summary>
        private readonly List<AvatarStatus> _statusList;
        /// <summary>
        /// Ensures that the avatar status is available both on disk, and in memory.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <returns></returns>
        private AvatarStatus EnsureStatus(Avatar avatar)
        {
            AvatarStatus status;
            var query = from AvatarStatus p in _db where p.Avatar.Citizen == avatar.Citizen select p;
            if (query.Count() == 0)
                status = new AvatarStatus(avatar);
            else
            {
                status = query.Single();
                status.Avatar = avatar;  // update old avatar status, citname might have been changed.
            }
            status.LastSeen = DateTime.Now;
            _db.Store(status);
            _db.Commit();
            _statusList.Add(status);
            return status;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChatBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public AfkBotPlugin(BotEngine bot): base(bot)
        {
            _statusList = new List<AvatarStatus>();
            // Get a dedicated client/server connection to the storage server.
            _db = bot.Storage.Clone();
            // reset status for all avatars.
            foreach (var avatar in bot.SceneNodes.Avatars)
                EnsureStatus(avatar);
            // The bot needs to attach to three events for operation.
            bot.AvatarEventAdd += AvatarEventAdd;
            bot.AvatarEventRemove += AvatarEventRemove;
            bot.ChatEvent += ChatEvent;
        }

        public override void PluginInitialized()
        {
            Bot.Say("afkbot initialized.");
        }

        /// <summary>
        /// Handles a chat event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void ChatEvent(BotEngine sender, EventChatArgs e)
        {
            var query = from AvatarStatus p in _statusList where p.Avatar.Citizen == e.Avatar.Citizen select p;
            var status = query.Single();
            status.LastSeen = DateTime.Now;
            var cmd = new CommandLine(e.Message); /* use a simple command line interpreter */
            switch (cmd.Command)
            {
                case "!seen":
                    if (cmd.Arguments.Count == 0)
                        sender.Whisper(e.Avatar,"Invalid, usage is in the form of !seen <citizen name>.");
                    else
                    {
                        var avatar = from AvatarStatus p in _statusList
                                    where p.Avatar.Name.ToLower() == cmd.Arguments[0].Value.Value.ToLower() select p;
                        if (avatar.Count() == 1)
                        {
                            sender.Whisper(e.Avatar, string.Format("Citizen {0} is here.", cmd.Arguments[0].Value.Value));
                            return;
                        }
                        var offline = from AvatarStatus p in _db where p.Avatar.Name.ToLower() == cmd.Arguments[0].Value.Value.ToLower() select p;
                        if (offline.Count() == 0)
                        {
                            sender.Whisper(e.Avatar, string.Format("Citizen {0} was never seen by me.", cmd.Arguments[0].Value.Value));
                            return;
                        }
                        var seen = offline.Single();
                        sender.Whisper(e.Avatar, string.Format("Citizen {0} was last seen on {1} at {2}", cmd.Arguments[0].Value.Value, seen.LastSeen.ToLongDateString(), seen.LastSeen.ToLongTimeString()));
                        return;
                    }
                    break;
                case "!idle" :
                    var message = string.Empty;
                    foreach (var avatar in _statusList)
                        if (avatar.IsIdle)
                            message += " " + avatar.Avatar.Name;
                    if (message == string.Empty)
                        sender.Whisper(e.Avatar,"No one is idle.");
                    else
                        sender.Whisper(e.Avatar, "Idle citizens:" + message);
                    break;
            }
        }

        /// <summary>
        /// Fired when an avatar leaves the world
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void AvatarEventRemove(BotEngine sender, EventAvatarRemoveArgs e)
        {
            var query = from AvatarStatus p in _statusList where p.Avatar.Citizen == e.Avatar.Citizen select p;
            var status = query.Single();
            status.LastSeen = DateTime.Now;
            _db.Store(status);
            _db.Commit();
            _statusList.Remove(status);
        }

        /// <summary>
        /// Fired when an avatar enters the world.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void AvatarEventAdd(BotEngine sender, EventAvatarAddArgs e)
        {
            EnsureStatus(e.Avatar);
        }

        /// <summary>
        /// Called by the plugin services manager.
        /// Releases unmanaged and - optionally - managed resources.
        /// You should clean up your references to Bot here, such as events.
        /// And flush your game state data.
        /// </summary>
        public override void Dispose()
        {
            // store the last seen value before shutting down the bot completely.
            foreach (var status in _statusList)
            {
                status.LastSeen = DateTime.Now;
                _db.Store(status);
            }
            _db.Commit();
            _db.Dispose(); /* cleanup db resources and close connection */

            Bot.AvatarEventAdd -= AvatarEventAdd;
            Bot.AvatarEventRemove -= AvatarEventRemove;
            Bot.ChatEvent -= ChatEvent;
            base.Dispose();
        }
    }
}