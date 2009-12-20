using AwManaged.ExceptionHandling;
using NUnit.Framework;

namespace Bot.Test
{
    [TestFixture]
    public class AwExceptionTests
    {
        [Test]
        public void DefaultTest()
        {
            int rc = 13;
            if (rc != 0)
            {
                var exception = new AwException(rc);
                Assert.IsTrue(exception.Rc == 13 && exception.Message == "Unable to login due to invalid password.");
            }
        }
    }
}