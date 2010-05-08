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
using AwManaged.Core.Interfaces;
using AwManaged.Scene;

namespace AwManaged.EventHandling.BotEngine
{
    [Serializable]
    public delegate void ChatEventDelegate(AwManaged.BotEngine sender, EventChatArgs e);

    public sealed class EventChatArgs : MarshalIndefinite
    {
        public EventChatArgs(ICloneableT<Avatar> avatar, ChatType chatType, string message)
        {
            Avatar = avatar.Clone();
            ChatType = chatType;
            Message = message;
        }

        public string Message{get; private set;}
        public ChatType ChatType {get;private set;}
        public Avatar Avatar { get; private set;}
    }
}