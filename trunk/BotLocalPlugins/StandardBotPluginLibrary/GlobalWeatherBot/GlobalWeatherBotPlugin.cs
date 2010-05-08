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
    /// Global Weather Bot. To demonstrate usng asyncrhonous web services using SOAP.
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("wbot", "This bot provides global weather info.")] /* plugin information for the plugin provider */
    public class GlobalWeatherBotPlugin : BotLocalPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChatBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public GlobalWeatherBotPlugin(BotEngine bot)
            : base(bot)
        {
            // The bot needs to attach to one events for operation.
            bot.ChatEvent += ChatEvent;
        }

        void ChatEvent(BotEngine sender, EventChatArgs e)
        {
            var cmd = new CommandLine(e.Message); /* use a simple command line interpreter */
            if (string.IsNullOrEmpty(cmd.Command) || cmd.Command != "!wbot")
                return;

            switch (cmd.Arguments[0].Value.Value)
            {
                case "cities":
                    var svc = new GlobalWeatherService.GlobalWeather();
                    string cities = svc.GetCitiesByCountry(cmd.Arguments[1].Value.Value);
                    sender.Whisper(e.Avatar, cities);
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