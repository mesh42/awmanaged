using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwManaged.Math;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class MathTests
    {
        private Vector3 GetCellFromVector3(Vector3 position)
        {
            return new Vector3((int)position.x/1000,0,(int)position.z/1000);
        }
        [Test]
        public void GetCellFromVector3Test()
        {
            var ret = GetCellFromVector3(new Vector3(3401, 2003, -102991));
        }
    }
}
