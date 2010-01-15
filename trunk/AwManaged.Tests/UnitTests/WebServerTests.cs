using AwManaged.LocalServices;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class WebServerTests
    {
        [Test]
        public void TestService()
        {
            var www = new WebServerService("provider=webserver;port=89");
            www.Start();

        }
    }
}
