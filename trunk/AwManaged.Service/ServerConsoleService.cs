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
using System.ServiceProcess;
using System.Threading;

namespace AwManaged.Service
{
    public partial class ServerConsoleService : ServiceBase
    {
        public ServerConsoleService()
        {
            InitializeComponent();
        }



        private Thread t;

        protected override void OnStart(string[] args)
        {
            BotEngine bot = new BotEngine();
            t = new Thread(bot.Start);
            t.Start();

        }

        protected override void OnStop()
        {
            t.Abort();
        }
    }
}
