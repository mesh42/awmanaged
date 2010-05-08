using SharedMemory;using System;
using System.Linq;
using System.Text.RegularExpressions;
using AwManaged;
using AwManaged.ConsoleServices;
using AwManaged.Core.Reflection.Attributes;
using AwManaged.Core.Scheduling;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using StandardBotPluginLibrary.QuoteBot.Rss;

namespace StandardBotPluginLibrary.QuoteBot
{
    [PluginInfo("qbot", "This is a simple quote bot.")] /* plugin information for the plugin provider */
    public class QuoteBotPlugin : BotLocalPlugin
    {
        private int _totalQuotes;
        private Random _random;
        private IObjectContainer _db;
        private RssReader _rss;
        private SchedulingItem _rssSchedulingItem;
        private SchedulingItem _quoteSchedulingItem;

        public QuoteBotPlugin(BotEngine bot) : base(bot){}
        private int _currentIndex = 0;

        public override void PluginInitialized()
        {
            _random = new Random();
            _db = Bot.Storage.Clone();
            try
            {
                _rss = new RssReader("http://quotes4all.net/rss/090010110/quotes.xml");
            }
            catch (Exception ex)
            {
                Bot.Console.Write(ConsoleMessageType.Error,"QuoteBot: Can't connect to rss feed.");
            }
            _totalQuotes = _db.Query(typeof(QuoteItem)).Count;
            // Add scheduling for polling the Rss feed.
            _rssSchedulingItem = new SchedulingItem();
            _rssSchedulingItem.Run.From(DateTime.Now.AddMinutes(1)).Every.Hours(2);
            Bot.SchedulingService.Submit(_rssSchedulingItem, RssSchedulingCallback);

            // Add scheduling for displaying a random quote.
            _quoteSchedulingItem = new SchedulingItem();
            _quoteSchedulingItem.Run.From(DateTime.Now.AddSeconds(30)).Every.Minutes(5);
            Bot.SchedulingService.Submit(_quoteSchedulingItem, QuoteSchedulingCallback);
            RssSchedulingCallback(null);
        }

        /// <summary>
        /// Rss Scheduling callback.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        private void RssSchedulingCallback(SchedulingItem schedule)
        {
            Bot.Console.WriteLine(string.Format("QuoteBot: Polling Rss service."));
            try
            {
                try
                {
                    _rss.Refresh();
                }
                catch
                {
                    Bot.Console.Write(ConsoleMessageType.Error, "QuoteBot: Can't connect to rss feed.");
                    return;
                }
                try
                {
                    foreach (var rssItem in _rss.Entries)
                    {
                        var quoteItem = new QuoteItem(_totalQuotes + 1,
                                                      Regex.Replace(rssItem.Description, @"(<[^>]+>)", string.Empty).
                                                          Trim());
                        if ((from QuoteItem p in _db where p.Hash == quoteItem.Hash select p).Count() != 0) continue;
                        _db.Store(quoteItem);
                        _db.Commit();
                        _totalQuotes++;
                    }
                }
                catch
                {
                    Bot.Console.Write(ConsoleMessageType.Error, "QuoteBot: Incorrect Rss format.");
                }
            }
            catch (Exception ex)
            {
                Bot.Console.WriteLine(ConsoleMessageType.Error, string.Format("QuoteBot: " + ex.Message));
            }
            Bot.Console.WriteLine(string.Format("QuoteBot: {0} quotes available in database.", _totalQuotes));
        }
        /// <summary>
        /// Quote Scheduling callback.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        private void QuoteSchedulingCallback(SchedulingItem schedule)
        {
            var id = _random.Next(1, _totalQuotes - 1);
            // fetch a random quote (simple, could be better by introducing statistics).
            var quote = (from QuoteItem p in _db where p.Id == id select p);
            var text = quote.Single().Quote;
            Bot.Console.WriteLine(string.Format("QuoteBot: {0}", text));
            Bot.ConsoleMessage(System.Drawing.Color.DarkBlue,false,true, "Quote: \""  + text + "\"");
        }

        public override void Dispose()
        {
            _rss.Dispose();
            Bot.SchedulingService.Cancel(_rssSchedulingItem.Id);
            Bot.SchedulingService.Cancel(_quoteSchedulingItem.Id);
            base.Dispose();
        }
    }
}
