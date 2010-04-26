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

namespace AwManaged.Core.Services
{
    public abstract class BaseConnection<TConnectionInterface> : IConnection<TConnectionInterface>
    {
        protected BaseConnection(string connection)
        {
            ConnectionString = connection;
        }

        #region IConnection<TConnectionInterface> Members

        public string ConnectionString { get; internal set; }

        #endregion
    }
}
