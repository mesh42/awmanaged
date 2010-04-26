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
using System.Linq;
using AwManaged;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.EventHandling.BotEngine;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using StandardBotPluginLibrary.GreeterBot;

namespace StandardBotPluginLibrary.StatsBot
{
    /// <summary>
    /// Simple stats bot. To demonstrate The Local Plugin Provider
    /// Part of the Standard Bot Plugin Library.
    /// </summary>
    [PluginInfo("statsbot", "This is a simple  statistics bot.")] /* plugin information for the plugin provider */
    public class StatsBotPlugin : BotLocalPlugin
    {
        private IObjectContainer _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="GreeterBotPlugin"/> class.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public StatsBotPlugin(BotEngine bot)
            : base(bot)
        {
            _db = bot.Storage.Clone();
            // The bot needs to attach to one event for operation.
            bot.ObjectEventClick += new ObjectEventClickDelegate(ObjectEventClick);
        }

        void ObjectEventClick(BotEngine sender, EventObjectClickArgs e)
        {
            lock (_db)
            {
                // store the number of clicks on this object in the storage provider.
                ModelClickStatistics stat;
                var query = from ModelClickStatistics p in _db where p.ModelId == e.Model.Id select p;
                if (query.Count() == 0)
                    stat = new ModelClickStatistics() {Clicks = 0, ModelId = e.Model.Id};
                else
                    stat = query.Single();
                lock (this)
                {
                    stat.Clicks++;
                    _db.Store(stat);
                    _db.Commit();

                    sender.Console.WriteLine(string.Format("object {0} with id {1} clicked by {2}. Total clicks {3}.",
                                                           e.Model.ModelName, e.Model.Id, e.Avatar.Name, stat.Clicks));
                }
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
            Bot.ObjectEventClick -= ObjectEventClick;
            base.Dispose();
        }
    }
}