/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
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
