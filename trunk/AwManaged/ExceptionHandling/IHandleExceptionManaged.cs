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
using AW;

namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// Manage exception handling, converts Active Worlds Reason Codes to verbose managed .NET exceptions.
    /// </summary>
    public interface IHandleExceptionManaged
    {
        /// <summary>
        /// Handles the exception managed.
        /// </summary>
        /// <param name="rc">The rc.</param>
        void HandleExceptionManaged(int rc);
        /// <summary>
        /// Handles the exception managed.
        /// </summary>
        /// <param name="instanceExcpetion">The instance exception.</param>
        void HandleExceptionManaged(InstanceException instanceExcpetion);
    }
}
