using System.IO;

namespace AwManaged.RwxTools.UnitTests
{
    using SharedMemory;using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleStatisticsTests
    {
        [Test]
        public void ConstructorTest()
        {
            var statistics = new RwxSimpleStatistics(new FileInfo(@".\RwxTools\UnitTests\UnitTestModel.rwx"));
        }
    }
}
