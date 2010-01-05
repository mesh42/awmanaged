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

namespace AwManaged.Scene.ActionInterpreter
{
    /// <summary>
    /// Specifies whether the cursor should be displayed.
    /// </summary>
    [ACEnumType]
    public enum CursorType
    {
        /// <summary>
        /// On OR true OR yes: show the mouse. 
        /// </summary>
        [ACEnumBinding(new[]{"on","true","yes"})]
        On,
        /// <summary>
        /// off OR false OR no: hide the mouse cursor.
        /// </summary>
        [ACEnumBinding(new[] { "off", "false", "no" })]
        Off
    }
}