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
using AwManaged.Core.Reflection.Attributes;
using AwManaged.EventHandling.BotEngine;
using AwManaged.Scene;

namespace StandardBotPluginLibrary.GreeterBot
{
    /// <summary>
    /// Simple greeter bot. To demonstrate The Local Plugin Provider
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("gbot","This is a simple greeterbot.")] /* plugin information for the plugin provider */
    public class GreeterBotPlugin :  BotLocalPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GreeterBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public GreeterBotPlugin(BotEngine bot) : base(bot)
        {
            // The bot needs to attach to two events for operation.
            bot.AvatarEventAdd += AvatarEventAdd;
            bot.AvatarEventRemove += AvatarEventRemove;
        }

        /// <summary>
        /// Fired when an avatar leaves the world
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void AvatarEventRemove(BotEngine sender, EventAvatarRemoveArgs e)
        {
            // Wait 5 seconds, before the leave message is sent. The message will not be send if the avatar reenters the world within that time.
            // This is to prevent message flooding.
            sender.Say(5000, SessionArgumentType.AvatarSessionMustNotExist,
                e.Avatar, string.Format("{0} has left {1}.", e.Avatar.Name, sender.LoginConfiguration.Connection.World));
        }

        /// <summary>
        /// Fired when an avatar enters the world.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        static void AvatarEventAdd(BotEngine sender, EventAvatarAddArgs e)
        {
            // Wait 5 seconds, before the greeting message is sent. The message will not be send if the avatar doesn't exist anymore at that time.
            // This is to prevent message flooding.
            sender.Say(5000, SessionArgumentType.AvatarSessionMustExist,
                e.Avatar,string.Format("{0} enters.", e.Avatar.Name));
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
            Bot.AvatarEventAdd -= AvatarEventAdd;
            Bot.AvatarEventRemove -= AvatarEventRemove;
            base.Dispose();
        }
    }
}
