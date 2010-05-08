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

namespace AwManaged.LocalServices.WebServer.Attributes
{
    public class WebFormFieldAttribute : Attribute
    {
        private readonly string _regexPattern;
        private readonly bool _isRequired;

        public WebFormFieldAttribute(string regexPattern, bool isRequired)
        {
            _regexPattern = regexPattern;
            _isRequired = isRequired;
        }

        public bool IsRequired
        {
            get { return _isRequired; }
        }

        public string RegexPattern
        {
            get { return _regexPattern; }
        }

    }
}
