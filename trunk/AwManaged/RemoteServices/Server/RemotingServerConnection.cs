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
using AwManaged.Core.Services;

namespace AwManaged.RemoteServices.Server
{
    public sealed class RemotingServerConnection : BaseConnection<RemotingServerConnection>
    {
        public int Port { get; internal set; }
        public RemotingProtocol Protocol { get; internal set; }

        public RemotingServerConnection()
            : base(null)
        {

        }

        public RemotingServerConnection(string connection)
            : base(connection)
        {
        }

    }
}