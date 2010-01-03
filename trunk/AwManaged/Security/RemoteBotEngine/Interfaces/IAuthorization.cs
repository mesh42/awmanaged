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
namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IAuthorization<TObject> where TObject : IHaveAuthorization
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        string Role { get; }
        /// <summary>
        /// Gets or sets the authorizable templated object.
        /// </summary>
        /// <value>The authorizable.</value>
        TObject Authorizable { get; }
        /// <summary>
        /// Adds the authorization.
        /// </summary>
        void AddAuthorization();

        bool HasAuthorization();
        /// <summary>
        /// Removes the authorization.
        /// </summary>
        void RemoveAuthorization();
    }
}