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
    public sealed class DependendObject<TParent, TChild> : ICloneableT<DependendObject<TParent, TChild>>
        where TParent : ICloneableT<TParent>
        where TChild : ICloneableT<TChild> 
    {
        private TParent Parent { get; set;}
        private TChild Child { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependendObject&lt;TParent, TChild&gt;"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        public DependendObject(TParent parent, TChild child)
        {
            Parent = parent;
            Child = child;
        }

        #region ICloneableT<DependendObject<TParent,TChild>> Members

        DependendObject<TParent, TChild> ICloneableT<DependendObject<TParent, TChild>>.Clone()
        {
            return (DependendObject<TParent, TChild>) MemberwiseClone();
        }

        #endregion
    }
}
