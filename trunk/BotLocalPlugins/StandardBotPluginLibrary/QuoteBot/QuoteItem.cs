using Db4objects.Db4o.Config.Attributes;

namespace StandardBotPluginLibrary.QuoteBot
{
    public class QuoteItem
    {
        [Indexed]
        private readonly int _id;
        [Indexed]
        private readonly int _hash;
        private readonly string _quote;

        public QuoteItem(int id, string quote)
        {
            _id = id;
            _quote = quote;
            _hash = quote.GetHashCode();
        }

        public string Quote
        {
            get { return _quote; }
        }

        public int Id
        {
            get { return _id; }
        }

        public int Hash
        {
            get { return _hash; }
        }
    }
}
