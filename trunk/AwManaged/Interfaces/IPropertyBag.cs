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
namespace AwManaged.Interfaces
{
    public interface IPropertyBag<TParent>
    {
        /// <summary>
        /// Gets or sets the parent object this property bag belongs to.
        /// </summary>
        /// <value>The parent.</value>
        TParent Parent { get; set; }
        object GetProperty(string key);
        void SetProperty(string key, object value);

    }
}