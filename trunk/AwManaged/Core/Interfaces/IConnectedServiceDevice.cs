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
namespace AwManaged.Core.Interfaces
{
    /// <summary>
    /// A Device that can be connected using a connection string and can stop and start its Services.
    /// </summary>
    /// <typeparam name="TConnectionInterface">The type of the connection interface.</typeparam>
    public interface IConnectedServiceDevice<TConnectionInterface> : 
        IService, IConnectionProperties<TConnectionInterface> where TConnectionInterface : IConnection<TConnectionInterface>
    {
        string ProviderName { get; }
    }
}
