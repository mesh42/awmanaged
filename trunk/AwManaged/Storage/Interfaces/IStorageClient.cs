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
using Db4objects.Db4o;

namespace AwManaged.Storage.Interfaces
{
    /// <summary>
    /// Storage Client.
    /// </summary>
    public interface IStorageClient<TConnectionInterface> : IConnectedServiceDevice<TConnectionInterface>, ICloneableT<IObjectContainer> 
        where TConnectionInterface : IConnection<TConnectionInterface>
    {
        /// <summary>
        /// Gets the Linq queryable db.
        /// </summary>
        /// <value>The db.</value>
        IObjectContainer Db { get; }
    }
}
