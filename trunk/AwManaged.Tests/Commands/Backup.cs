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
using System.Collections.Generic;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Tests.Commands
{
    [CCHelpDescription("Backup command group, providing access to several backup providers.")]
    public sealed class Backup : ICommandGroup
    {
        #region ITrigger Members

        public string LiteralAction
        {
            get { return "backup"; }
        }

        #endregion

        #region IActionTrigger Members

        public string LiteralCommands
        {
            get;
            set;
        }

        public List<IActionCommand> Commands
        {
            get;
            set;
        }

        #endregion

        public string LiteralPart { get; set; }

    }
}
