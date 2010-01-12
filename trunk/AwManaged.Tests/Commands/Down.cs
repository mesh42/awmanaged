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
using AwManaged.Scene.ActionInterpreter.Attributes;

namespace AwManaged.Tests.Commands
{
    /// <summary>
    /// Lists the directory.
    /// </summary>
    public class Down
    {
        [Attributes.CCItemBinding(CommandInterpretType.SingleArgument)]
        public string ServiceName { get; set; }
    }
}
