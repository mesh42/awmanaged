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
namespace AwManaged.EventHandling.BotEngine.Interfaces
{
    public interface IBotEngineEvents
    {
        event BotEventLoggedInDelegate BotEventLoggedIn;
        event BotEventEntersWorldDelegate BotEventEntersWorld;
        event AvatarEventAddDelegate AvatarEventAdd;
        event AvatarEventChangeDelegate AvatarEventChange;
        event AvatarEventRemoveDelegate AvatarEventRemove;
        event ObjectEventClickDelegate ObjectEventClick;
        event ObjectEventAddDelegate ObjectEventAdd;
        event ObjectEventRemoveDelegate ObjectEventRemove;
        event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        event ChatEventDelegate ChatEvent;
        event ObjectEventChangeDelegate ObjectEventChange;
    }
}
