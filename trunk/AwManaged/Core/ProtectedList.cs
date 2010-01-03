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

namespace AwManaged.Core
{
    /// <summary>
    /// Provides a list with minimal functionality exposed to prevent abuse of the AWManaged cache's.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ProtectedList<T> : BaseCacheList<T,ProtectedList<T>> where T : ICloneableT<T>
    {
        public override ProtectedList<T> Clone()
        {
            return (ProtectedList<T>) MemberwiseClone();
        }
    }
}
