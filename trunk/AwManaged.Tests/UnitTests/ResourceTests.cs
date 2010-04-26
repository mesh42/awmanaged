using System.Globalization;
using System.Threading;
using AwManaged.Core.ServicesManaging;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void GetResourceStringTest()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
            var manager = new ServicesManager();
            manager.StartService("bla");
        }
    }
}
