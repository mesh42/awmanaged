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
using SharedMemory;using System;

namespace AwManaged.Core.Commanding.Attributes
{
    public class CCGroupBindingAttribute : Attribute
    {
        private readonly Type[] _groupCommandTypes;

        public CCGroupBindingAttribute(Type[] groupCommandTypes)
        {
            _groupCommandTypes = groupCommandTypes;
        }

        public Type[] GroupCommandTypes
        {
            get { return _groupCommandTypes; }
        }
    }
}