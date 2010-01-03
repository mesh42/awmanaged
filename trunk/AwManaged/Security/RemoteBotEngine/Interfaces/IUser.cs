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
using Db4objects.Db4o;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IUser : IHaveAuthorization
    {
        string Name { get; }
        string Password { get; }
        string LastLogonDate { get; }
        string Email { get; }
        string NameToLower { get; }
        string EmailToLower { get; }
        bool IsLockedOut { get; }
        int MaxConnections { get; }
        /// <summary>
        /// Registers the user using the specified storage client.
        /// </summary>
        /// <returns></returns>
        RegistrationResult RegisterUser(IObjectContainer storage);
        /// <summary>
        /// Determines whether the specified user is authenticated.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <returns>
        /// 	<c>true</c> if the specified storage is authenticated; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthenticated(IObjectContainer storage);
    }
}