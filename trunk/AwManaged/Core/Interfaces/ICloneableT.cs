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
    /// Indicates an object template is cloneable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneableT<T>
    {
        /// <summary>
        /// Clones this instance to prevent dirty reads and writes on the original object.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}