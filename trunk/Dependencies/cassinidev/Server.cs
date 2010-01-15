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
using System.Net;
using System.Threading;
using CassiniDev;

namespace Cassini
{
    /// <summary>
    /// 
    /// Please put all but the most trivial changes and all additions to Server in partial files to
    /// reduce the code churn and pain of merging new releases of Cassini. If a method is to be significantly modified,
    /// comment it out, explain the modification/move in the header and put the modified version in this file.
    /// 
    /// 12/29/09 sky: Implemented IDisposable to help eliminate zombie ports
    /// 12/29/09 sky: Added instance properties for HostName and IPAddress and constructor to support them
    /// 12/29/09 sky: Extracted and implemented IServer interface to facilitate stubbing for tests
    /// 
    /// </summary>
    public partial class Server : IServer
    {
        private bool _disposed;
        private string _hostName;
        private IPAddress _ipAddress;
        private int _requestCount;
        private int _timeout;
        private Timer _timer;

        public Server(ServerArguments args)
            : this(args.Port, args.VirtualPath, args.ApplicationPath)
        {
            _ipAddress = args.IPAddress;
            _hostName = args.Hostname;
            _timeout = args.TimeOut;
        }

        #region IServer Members

        public event EventHandler Stopped;

        public string HostName
        {
            get { return _hostName; }
        }

        public IPAddress IPAddress
        {
            get { return _ipAddress; }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                // just add a little slack for the socket transition to TIME_WAIT
                Thread.Sleep(10);
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        ~Server()
        {
            Dispose();
        }

        #endregion

        private void InvokeStopped()
        {
            EventHandler handler = Stopped;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void IncrementRequestCount()
        {
            _requestCount++;
            _timer = null;
        }

        private void TimedOut(object ignored)
        {
            
            Stop();
        }

        private void DecrementRequestCount()
        {
            _requestCount--;
            
            if (_requestCount < 1)
            {
                _requestCount = 0;

                

                if (_timeout > 0)
                {
                    // start timer
                    _timer = new Timer(TimedOut, null, _timeout, Timeout.Infinite);
                }
            }
        }
    }
}