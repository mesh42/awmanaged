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
using AwManaged.Core.Services.Interfaces;
using AwManaged.Storage.Interfaces;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IIdentityManagementClient<TConnectionInterface> : IIdentityManagementObjects, IService
        where TConnectionInterface : IConnection<TConnectionInterface>
    {
        IStorageClient<TConnectionInterface> Storage { get; }
    }
}
