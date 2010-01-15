using System;
using System.Collections.Generic;
using System.Text;

namespace StandardBotPluginLibrary.QuoteBot.Rss
{
    public class RssEntry
    {
        string title;
        string link;
        string description;

        /// <summary>
        /// Gets the Title.
        /// </summary>
        /// <value>The Title.</value>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// Gets the Link.
        /// </summary>
        /// <value>The Link.</value>
        public string Link
        {
            get { return link; }
        }

        /// <summary>
        /// Gets the Description.
        /// </summary>
        /// <value>The Description.</value>
        public string Description
        {
            get { return description; }
        }


        public RssEntry(string title, string link, string description)
        {
            this.title = title;
            this.link = link;
            this.description = description;
        }
    }
}