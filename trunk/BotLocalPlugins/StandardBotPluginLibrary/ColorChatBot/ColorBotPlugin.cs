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
using SharedMemory;using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AwManaged;
using AwManaged.Core.Commanding;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.EventHandling;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Scene;
using Db4objects.Db4o.Linq;

namespace StandardBotPluginLibrary.ColorChatBot
{
    /// <summary>
    /// Color Chat Bot. To demonstrate The Local Plugin Provider
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("ccbot","This is a color chat bot")] /* plugin information for the plugin provider */
    public class ColorChatBotPlugin :  BotLocalPlugin
    {
        private List<AvatarColorChatSetting> _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChatBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public ColorChatBotPlugin(BotEngine bot) : base(bot)
        {
            _settings = new List<AvatarColorChatSetting>();

            // The bot needs to attach to three events for operation.
            bot.AvatarEventAdd += AvatarEventAdd;
            bot.AvatarEventRemove += AvatarEventRemove;
            bot.ChatEvent += ChatEvent;
            bot.IsEchoChat = false;
            
            /* PurgeColors(); */ // used for testing.
            foreach(Avatar avatar in bot.SceneNodes.Avatars)
            {
                LoadColorSetting(avatar);
                Bot.Whisper(avatar, "Color Chat Bot is online, whisper '!ccbot', for help.");
            }
        }

        /// <summary>
        /// Purges all the avatar color chat settings from the database.
        /// Use this for testing if you need to cleanup something.
        /// </summary>
        private void PurgeColors()
        {
            foreach (var color in from AvatarColorChatSetting p in Bot.Storage.Db select p)
            {
                Bot.Storage.Db.Delete(color);
                Bot.Storage.Db.Commit();
            }
        }

        /// <summary>
        /// Loads the color setting for the specified avatar and creates a new color setting in the datbase if needed.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        private void LoadColorSetting(Avatar avatar)
        {
            var setting = from AvatarColorChatSetting p in Bot.Storage.Db where (p.Citizen == avatar.Citizen) select p;
            AvatarColorChatSetting memSetting;
            if (setting.Count() == 0)
            {
                memSetting = new AvatarColorChatSetting(avatar.Citizen, false, false, Color.Black);
                Bot.Storage.Db.Store(memSetting);
                Bot.Storage.Db.Commit();
            }
            else
            {
                memSetting = setting.Single();
            }
            _settings.Add(memSetting);
        }

        /// <summary>
        /// Handles the incoming chat event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ChatEvent(BotEngine sender, EventChatArgs e)
        {
            switch (e.ChatType)
            {
                case ChatType.Normal: 
                case ChatType.Broadcast:
                    var setting =
                        (from AvatarColorChatSetting p in _settings where (p.Citizen == e.Avatar.Citizen) select p).
                            Single();
                    var name = e.Avatar.Name + ":".PadRight(31 - e.Avatar.Name.Length);
                    Bot.ConsoleMessage(e.Avatar.Name,setting.Color, setting.IsBold, setting.IsItalic,e.Message);
                    break;
                case ChatType.Whisper: 
                    var cmd = new CommandLine(e.Message.ToLower()); /* use a simple command line interpreter */
                    switch (cmd.Command)
                    {
                        case "!ccbot":
                            if (cmd.Arguments.Count != 1)
                            {
                                sender.Whisper(e.Avatar,
                                               "Use in the form of: !ccbot <#color> where #color is in the form of a HTML color. I.e: #000000 (black) #FF0000 red etc. You can also use known color names, such as Red, Blue, DarkViolet etc.");
                            }
                            else
                            {
                                var currentColor = (from AvatarColorChatSetting p in _settings
                                     where (p.Citizen == e.Avatar.Citizen)
                                     select p).Single();
                                try
                                {
                                    currentColor.Color = ColorTranslator.FromHtml(cmd.Arguments[0].Value.Value.ToLower());
                                    var dbColor = (from AvatarColorChatSetting p in sender.Storage.Db where (p.Citizen == e.Avatar.Citizen)
                                                  select p).Single();
                                    dbColor.Color = currentColor.Color; /* todo: this object reference pattern should be genericly embedded within BotEngine */
                                    sender.Storage.Db.Store(dbColor);
                                    sender.Storage.Db.Commit();
                                    Bot.Whisper(e.Avatar, string.Format("Your chat color has been changed to '{0}' brightness {1}", cmd.Arguments[0].Value.Value,dbColor.Color.GetBrightness()));
                                }
                                catch
                                {
                                    Bot.Whisper(e.Avatar,string.Format("The chat color '{0}'is not recognized. Please try another color.",cmd.Arguments[0].Value.Value));
                                }
                            }

                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Fired when an avatar leaves the world. It removes the color setting from the in memory generic list.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void AvatarEventRemove(BotEngine sender, EventAvatarRemoveArgs e)
        {
            _settings.Remove((from AvatarColorChatSetting p in _settings where p.Citizen == e.Avatar.Citizen select p).Single());
        }

        /// <summary>
        /// Fired when an avatar enters the world. It loads the stored color setting for the avatar.
        /// It whispers to the citizen on how to use the color bot.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void AvatarEventAdd(BotEngine sender, EventAvatarAddArgs e)
        {
            LoadColorSetting(e.Avatar);
            Bot.Whisper(e.Avatar, "Color Chat Bot is online, whisper '!ccbot', for help.");
        }

        /// <summary>
        /// Called by the plugin services manager.
        /// Releases unmanaged and - optionally - managed resources.
        /// You should clean up your references to Bot here.
        /// </summary>
        public override void Dispose()
        {
            Bot.AvatarEventAdd -= AvatarEventAdd;
            Bot.AvatarEventRemove -= AvatarEventRemove;
            Bot.ChatEvent -= ChatEvent;
            Bot.IsEchoChat = true;
            base.Dispose();
        }
    }
}
