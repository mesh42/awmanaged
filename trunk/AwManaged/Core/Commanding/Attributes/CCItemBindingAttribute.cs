﻿/* **********************************************************************************
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
using AwManaged.Core.Interfaces;
using AwManaged.Scene.ActionInterpreter.Attributes;

namespace AwManaged.Core.Commanding.Attributes
{
    /// <summary>
    /// Contains information, on how to bind the command item in the action string to the Action object.
    /// </summary>
    public sealed class CCItemBindingAttribute : Attribute, ICommandItemBindingAttribute
    {
        public CommandInterpretType Type { get; set; }
        public string LiteralName { get; set; }
        public char Delimiter { get; set; }

        public CCItemBindingAttribute(string literalName, CommandInterpretType type)
        {
            LiteralName = literalName;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ACItemBindingAttribute"/> class.
        /// used for flag intepretation.
        /// </summary>
        /// <param name="type">The type.</param>
        public CCItemBindingAttribute(CommandInterpretType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ACItemBindingAttribute"/> class.
        /// for a name value pair, which property value is delimited with multi values.
        /// for example: 
        /// bump lock owners=4711:1174:333333
        /// </summary>
        /// <param name="literalName">Name of the literal.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="type">The type.</param>
        public CCItemBindingAttribute(string literalName, char delimiter, CommandInterpretType type)
        {
            LiteralName = literalName;
            Delimiter = delimiter;
            Type = type;
        }
    }
}