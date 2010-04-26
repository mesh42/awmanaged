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
using System.Net;
using AwManaged.Core.Services;

namespace AwManaged.LocalServices
{
    public sealed class WebServerConnection : BaseConnection<WebServerConnection>
    {
        public int Port { get; internal set; }
        public IPAddress IpAddress { get; internal set; }

        public WebServerConnection() : base(null)
        {
            
        }

        public WebServerConnection(string connection) : base(connection)
        {
        }

    }
}