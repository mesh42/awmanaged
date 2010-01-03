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
using System.Threading;

namespace AwManaged.Core.Interfaces
{
    /// <summary>
    /// Template for callbacks in the engine.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICallbackStructT<T> where T : ICloneableT<T>
    {
        /// <summary>
        /// Gets the cloned object.
        /// </summary>
        /// <value>The clone.</value>
        T Clone { get; }
        /// <summary>
        /// Gets the timer.
        /// </summary>
        /// <value>The timer.</value>
        Timer Timer { get; }
        /// <summary>
        /// Gets the optional parameters.
        /// </summary>
        /// <value>The param.</value>
        object[] Param { get; }
    }
}
