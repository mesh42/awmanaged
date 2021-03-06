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
using System.Drawing;
using AwManaged.Scene;
using AwManaged.Scene.Interfaces;
using AWManaged.Security;

namespace AwManaged.Interfaces
{
    public interface IChatCommands<TAvatar> where TAvatar : MarshalIndefinite, IAvatar<TAvatar>
    {
        /// <summary>
        /// Whispers a message to all user within a specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="message">The message.</param>
        void Whisper(RoleType role, string message);
        /// <summary>
        /// Whispers a message to the a specific avatar
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        void Whisper(TAvatar avatar, string message);
        /// <summary>
        /// Sends a specified message to the chatroom.
        /// </summary>
        /// <param name="message">The message.</param>
        void Say(string message);
        /// <summary>
        /// Sends a message to the chat room with a specified delay.
        /// If at the time the specified avatar session is not available,
        /// the message will be not be echoed to the chat room.
        /// Great for Greeter bots to prevent greeting message flooding.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="sessionArgumentType">Type of the session argument.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        void Say(int delay, SessionArgumentType sessionArgumentType, TAvatar avatar, string message);

        /// <summary>
        /// Sends a console message to the chat room with a specified delay.
        /// If at the time the specified avatar session is not available,
        /// the message will be not be echoed to the chat room.
        /// Great for Greeter bots to prevent greeting message flooding.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="argumentType">Type of the argument.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="color">The color.</param>
        /// <param name="isBold">if set to <c>true</c> [is bold].</param>
        /// <param name="isItalic">if set to <c>true</c> [is italic].</param>
        /// <param name="message">The message.</param>
        void ConsoleMessage(int delay, SessionArgumentType argumentType, TAvatar avatar, Color color, bool isBold, bool isItalic, string message);
    }
}
