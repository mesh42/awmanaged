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
using AwManaged.Core.Interfaces;
using AwManaged.Storage.Interfaces;

namespace AwManaged.Storage
{
    public class Db4OConnection : IConnection<Db4OConnection>
    {
        #region IDb4OConnection Members

        public string HostAddress
        {
            get; internal set;
        }

        public int HostPort
        {
            get;
            internal set;
        }

        public string File
        {
            get;
            internal set;
        }

        public string User
        {
            get;
            internal set;
        }

        public string Password
        {
            get;
            internal set;
        }

        #endregion

        #region IConnection<Db4OConnection> Members

        public string ConnectionString
        {
            get; internal set;
        }

        #endregion
    }
}
