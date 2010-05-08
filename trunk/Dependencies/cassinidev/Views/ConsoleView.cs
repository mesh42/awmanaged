// /* **********************************************************************************
//  *
//  * Copyright (c) Sky Sanders. All rights reserved.
//  * 
//  * This source code is subject to terms and conditions of the Microsoft Public
//  * License (Ms-PL). A copy of the license can be found in the license.htm file
//  * included in this distribution.
//  *
//  * You must not remove this notice, or any other, from this software.
//  *
//  * **********************************************************************************/
using SharedMemory;using System;

namespace CassiniDev.Views
{
    public class ConsoleView : IView
    {
        #region IView Members

        public IPresenter Presenter { get; set; }
        public int TimeOut { get; set; }
        public int WaitForPort { get; set; }

        public string ApplicationPath { get; set; }

        public string VirtualPath { get; set; }

        public string HostName { get; set; }

        public string IPAddress { get; set; }

        public bool IPv6 { get; set; }

        public bool AddHost { get; set; }

        public ushort Port { get; set; }

        public ushort PortRangeStart { get; set; }

        public ushort PortRangeEnd { get; set; }

        public string RootUrl { get; set; }

        public PortMode PortMode { get; set; }

        public IPMode IPMode { get; set; }

        public RunState RunState { get; set; }

        public void ClearError()
        {
            //noop;
        }

        public void SetError(ErrorField field, string message)
        {
            throw new CassiniException(message, field);
        }

        public void Start()
        {
            CommandLineArguments args = new CommandLineArguments
                                            {
                                                AddHost = AddHost,
                                                ApplicationPath = ApplicationPath,
                                                HostName = HostName,
                                                IPAddress = IPAddress,
                                                IPMode = IPMode,
                                                IPv6 = IPv6,
                                                Port = Port,
                                                PortMode = PortMode,
                                                PortRangeEnd = PortRangeEnd,
                                                PortRangeStart = PortRangeStart,
                                                VirtualPath = VirtualPath
                                            };

            Presenter.Start(args);
        }

        public void Stop()
        {
            Presenter.Stop(AddHost);
            Environment.Exit(-3);
            // todo:should notify host?
        }

        #endregion
    }
}