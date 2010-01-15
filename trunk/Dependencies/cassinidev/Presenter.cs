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
using System;
using System.IO;
using System.Net;

namespace CassiniDev
{
    public class Presenter : IPresenter
    {
        private bool _disposed;
        private IServer _server;
        private IView _view;

        #region IPresenter Members

        public IServer Server
        {
            get { return _server; }
        }

        public IView View
        {
            get { return _view; }
        }

        public void InitializeView(IView view, CommandLineArguments args)
        {
            _view = view;
            _view.Presenter = this;
            _view.RunState = RunState.Idle;

            _view.AddHost = args.AddHost;
            if(!string.IsNullOrEmpty(args.ApplicationPath))
            {
                args.ApplicationPath = args.ApplicationPath.Trim('\"').TrimEnd('\\');
            }

            _view.ApplicationPath = args.ApplicationPath;
            if(!string.IsNullOrEmpty(args.VirtualPath))
            {
                args.VirtualPath = args.VirtualPath.Trim('\"');
            }
            _view.VirtualPath = args.VirtualPath;
            _view.HostName = args.HostName;
            _view.IPAddress = args.IPAddress;
            _view.IPMode = args.IPMode;
            _view.IPv6 = args.IPv6;
            _view.Port = args.Port;
            _view.PortMode = args.PortMode;
            _view.PortRangeEnd = args.PortRangeEnd;
            _view.PortRangeStart = args.PortRangeStart;
            _view.RootUrl = string.Empty;
            _view.WaitForPort = args.WaitForPort;
            _view.TimeOut = args.TimeOut;

            try
            {
                ServiceFactory.Rules.ValidateArgs(args);
                // if an app path was passed, user wanted to start server
                if (!string.IsNullOrEmpty(args.ApplicationPath))
                {
                    Start(args);
                }
            }
            catch (CassiniException ex)
            {
                _view.SetError(ex.Field, ex.Message);
            }
        }

        public void Start(CommandLineArguments args)
        {
            _view.ClearError();

            try
            {
                ServiceFactory.Rules.ValidateArgs(args);
            }
            catch (CassiniException ex)
            {
                _view.SetError(ex.Field, ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(args.ApplicationPath) || !Directory.Exists(args.ApplicationPath))
            {
                _view.SetError(ErrorField.ApplicationPath, "Invalid Application Path");
                return;
            }

            // prepare arguments
            IPAddress ip = ServiceFactory.Rules.ParseIP(args.IPMode, args.IPv6, args.IPAddress);
            _view.IPAddress = ip.ToString();
            ushort port = args.Port;
            if (args.PortMode == PortMode.FirstAvailable)
            {
                port = ServiceFactory.Rules.GetAvailablePort(args.PortRangeStart, args.PortRangeEnd, ip, true);
            }
            _view.Port = port;
            string hostname = args.HostName;
            //if (string.IsNullOrEmpty(hostname))
            //{
            //    hostname = ip.ToString();
            //}

            _view.HostName = hostname;

            _server =
                ServiceFactory.CreateServer(new ServerArguments
                                                {
                                                    Port = port,
                                                    VirtualPath = args.VirtualPath,
                                                    ApplicationPath = args.ApplicationPath,
                                                    IPAddress = ip,
                                                    Hostname = hostname,
                                                    TimeOut = args.TimeOut
                                                });

            if (args.AddHost)
            {
                ServiceFactory.Rules.AddHostEntry(_server.IPAddress.ToString(), _server.HostName);
            }
            try
            {
                _server.Start();
                _server.Stopped += ServerStopped;
                _view.RootUrl = _server.RootUrl;
                _view.RunState = RunState.Running;
            }
            catch (Exception ex)
            {
                _view.SetError(ErrorField.None, ex.Message);
                _server.Dispose();
            }
        }

        public void Stop(bool removeHosts)
        {
            _view.RunState = RunState.Idle;
            if (removeHosts)
            {
                ServiceFactory.Rules.RemoveHostEntry(_server.IPAddress.ToString(), _server.HostName);
            }
            if (_server != null)
            {
                _server.Stopped -= ServerStopped;
                _server.Dispose();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_server != null)
                {
                    _server.Dispose();
                }
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        ~Presenter()
        {
            Dispose();
        }

        #endregion

        private void ServerStopped(object sender, EventArgs e)
        {
            _view.Stop();
        }
    }
}