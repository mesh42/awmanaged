using Db4objects.Db4o.Config.Attributes;

namespace AwManaged.Tests
{
    /// <summary>
    /// This is a simple helper class, for storing statistics on clicked objects.
    /// </summary>
    public class ModelClickStatistics
    {
        [Indexed]
        private int _clicks;
        [Indexed]
        private int _modelId;

        public int Clicks { get { return _clicks; } set { _clicks = value; } }
        public int ModelId { get { return _modelId; } set { _modelId = value; } }
    }
}
