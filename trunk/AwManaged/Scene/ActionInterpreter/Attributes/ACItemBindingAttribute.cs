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

namespace AwManaged.Scene.ActionInterpreter.Attributes
{
    /// <summary>
    /// Determines how the command item should be interpreted.
    /// </summary>
    public enum CommandInterpretType
    {
        /// <summary>
        /// Format is in name value pairs.
        /// </summary>
        NameValuePairs,
        /// <summary>
        /// Property values are seperated by spaces.
        /// </summary>
        Space
    }

    /// <summary>
    /// Contains information, on how to bind the command item in the action string to the Action object.
    /// </summary>
    public sealed class ACItemBindingAttribute : Attribute
    {
        public CommandInterpretType Type { get; set; }
        public string LiteralName { get; set; }

        public ACItemBindingAttribute(string literalName, CommandInterpretType type)
        {
            LiteralName = literalName;
            Type = type;
        }
    }
}
