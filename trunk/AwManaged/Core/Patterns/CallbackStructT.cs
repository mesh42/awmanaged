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

namespace AwManaged.Core.Patterns
{
    public sealed class CallbackStructT<T> : ICallbackStructT<T> where T : ICloneableT<T>
    {
        #region ICallbackStructT<T> Members

        public T Clone
        {
            get; private set;
        }

        public System.Threading.Timer Timer
        {
            get;
            internal set;
        }

        public object[] Param
        {
            get; internal set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackStructT&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="clone">The clone.</param>
        public CallbackStructT(T clone)
        {
            Clone = clone;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackStructT&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="clone">The clone.</param>
        /// <param name="param">The param.</param>
        public CallbackStructT(T clone, object[] param)
        {
            Clone = clone;
            Param = param;
        }

        #endregion
    }
}