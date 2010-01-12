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
using AwManaged.ConsoleServices;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class PromptInterpreterTests
    {
        [Test]
        public void RegexTest1()
        {
            var result = new CommandLine("down \"remote server\" notify=   all    label =  \"my label1\" /force /exit");
        }
    }

    public interface ICommandPart
    {
    }
}
