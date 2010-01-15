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
using System;
using System.IO;
using System.Reflection;
using AwManaged.Core.Scheduling;
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

        private class ScheduledClass
        {
        }

        public void CanceleSchedulingTest()
        {
            SchedulingService svc = new SchedulingService();

        }

        [Test]
        public void DiscoverBotPLuginsTest()
        {
            var types = BotLocalPlugin.Discover(new DirectoryInfo(Directory.GetCurrentDirectory()));
            //var info = BotLocalPlugin.GetPluginInfo(types[0]);

            //Assembly asm = Assembly.GetAssembly(types[0].Type);

            // create instance.
            //var constructor = asm.GetType(types[0].ToString()).GetConstructor(new[] { typeof(BotEngine) });
            //var constructor = types[0].GetConstructor(new[] {typeof (BotEngine)});
            //var plugin = constructor.Invoke(new object[]{null});

        }
    }
}
