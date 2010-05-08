using SharedMemory;using System;
using System.Configuration;
using System.Threading;
using AwManaged.Core.ServicesManaging;
using AwManaged.RemoteServices;
using AwManaged.RemoteServices.Server;
using AwManaged.Security.RemoteBotEngine;
using AwManaged.Storage;

namespace AwManaged.Tests.UnitTests
{
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for testing Remote Connections using .NET remoting.
    /// </summary>
    [TestFixture]
    public class RemotingTests
    {
        private BotEngine _engine;
        private RemotingServer<RemotingBotEngine> _remotingServer;
        private EventWaitHandle waitHandle;

        /// <summary>
        /// Starts the remoting test.
        /// </summary>
        [Test]
        public void StartRemotingTest()
        {
            waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _engine = new BotEngine();
            _engine.BotEventEntersWorld += new EventHandling.BotEngine.BotEventEntersWorldDelegate(_engine_BotEventEntersWorld);
            var engineThread = new Thread(_engine.Start);
            engineThread.Start();
            waitHandle.WaitOne();
        }

        void _engine_BotEventEntersWorld(BotEngine sender, EventHandling.BotEngine.EventBotEntersWorldArgs e)
        {
            // bot is logged in and entered the world. Create a remote client seeion to the bot engine.
            waitHandle.Set();
        }
    }
}
