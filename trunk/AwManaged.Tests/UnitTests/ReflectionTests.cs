using AwManaged.Interfaces;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    /// <summary>
    /// Reflection testing.
    /// </summary>
    [TestFixture]
    public class ReflectionTests
    {
        /// <summary>
        /// Gather all events of an interface.
        /// </summary>
        [Test]
        public void InterfaceEventReflectionTest()
        {
            var t = typeof (BotEngine);
            foreach (var e in t.GetEvents())
            {
            }
        }
    }
}
