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
namespace AwManaged.EventHandling
{
    /// <summary>
    /// Enumerates the different chat types that are available in the aw managed library.
    /// </summary>
    public enum ChatType : byte
    {
        /// <summary>
        /// A normal chat message from a certain avatar.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// A chat message received from a public speaker.
        /// </summary>
        Broadcast = 0,
        /// <summary>
        /// A chat message sent only to you a certain avatar.
        /// </summary>
        Whisper = 2
    }
}
