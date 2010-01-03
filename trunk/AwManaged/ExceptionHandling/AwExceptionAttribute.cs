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
using System;

namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// Attribute describing the RC enumeration, in a human readable format.
    /// </summary>
    public sealed class AwExceptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the message in the human readable format.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}