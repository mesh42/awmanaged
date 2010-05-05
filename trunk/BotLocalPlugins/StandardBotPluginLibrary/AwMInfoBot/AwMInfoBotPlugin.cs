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
using AwManaged;
using AwManaged.Core.Commanding;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.EventHandling.BotEngine;
using StandardBotPluginLibrary.GreeterBot;

namespace StandardBotPluginLibrary.AwMInfoBot
{
    /// <summary>
    /// Simple greeter bot. To demonstrate The Local Plugin Provider
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("awmbot", "This bot provides version info of awmanged.")] /* plugin information for the plugin provider */
    public class AwMInfoBotPlugin : BotLocalPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChatBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public AwMInfoBotPlugin(BotEngine bot)
            : base(bot)
        {
            // The bot needs to attach to one events for operation.
            bot.ChatEvent += ChatEvent;
        }

        void ChatEvent(BotEngine sender, EventChatArgs e)
        {
            var cmd = new CommandLine(e.Message); /* use a simple command line interpreter */
            if (string.IsNullOrEmpty(cmd.Command) || cmd.Command != "awm" || cmd.Arguments.Count==0)
                return;

            switch (cmd.Arguments[0].Value.Value)
            {
                case "version" :
                    sender.Say(string.Format("Managed Bot Engine Server {0}", sender.Version()));
                    break;
            }

        }

        public override void PluginInitialized()
        {
            base.PluginInitialized();
        }

        /// <summary>
        /// Called by the plugin services manager.
        /// Releases unmanaged and - optionally - managed resources.
        /// You should clean up your references to Bot here.
        /// </summary>
        public override void Dispose()
        {
            Bot.ChatEvent -= ChatEvent;
            base.Dispose();
        }
    }
}