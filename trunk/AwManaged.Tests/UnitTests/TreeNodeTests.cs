using AwManaged.Configuration;
using AwManaged.Core.Patterns.Tree;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class TreeNodeTests
    {
        [Test]
        public void TreeNodeTest()
        {
            var connection = new UniverseConnectionProperties() {World = "zebrakey"};
            connection.IdentifyableDisplayName = connection.World;
            var tree = new Tree();
            tree.Root.Children.Add(connection);
        }
    }
}
