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