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
namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// The cursor command specifies whether or not the mouse cursor should be displayed.
    /// </summary>
    public class ACCursor
    {
        private CursorType _flag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandCursor"/> class.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public ACCursor(CursorType flag)
        {
            _flag = flag;
        }

        /// <summary>
        /// The flag argument is required and specifies whether the cursor should be displayed.
        /// </summary>
        /// <value>The flag.</value>
        public CursorType Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
