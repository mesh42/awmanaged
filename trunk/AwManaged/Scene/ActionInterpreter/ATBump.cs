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
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Scene.ActionInterpreter
{
    public sealed class ATBump : ICommandGroup
    {

        #region ITrigger Members

        public string LiteralAction
        {
            get { return "bump"; }
        }

        #endregion


        #region IActionTrigger Members

        public List<IActionCommand> Commands
        {
            get;
            set;
        }

        #endregion

        public string LiteralPart { get; set; }

    }
}
