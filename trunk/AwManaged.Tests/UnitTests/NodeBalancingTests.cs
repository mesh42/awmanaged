using AwManaged.Configuration;
using AwManaged.Scene;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class NodeBalancingTests
    {
        [Test]
        public void BalancingActivatorTest()
        {
            var node1 = new BotNode<UniverseConnectionProperties, Model>(new UniverseConnectionProperties(), 1);
            var node2 = new BotNode<UniverseConnectionProperties, Model>(new UniverseConnectionProperties(), 2);
            node1.Connect();
            node1.Login();
            node2.Connect();
            node2.Login();
        }
    }
}
