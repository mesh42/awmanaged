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
using AwManaged.Configuration.Interfaces;
using AwManaged.EventHandling.Templated;

namespace AwManaged.EventHandling.Interfaces
{
    public interface IBotEvents<TSender, TConnectionProperties>
        where TConnectionProperties : MarshalByRefObject, IUniverseConnectionProperties<TConnectionProperties>
    {
        /// <summary>
        /// Occurs when [bot enters world].
        /// </summary>
        event BotEventEntersWorldDelegate<TSender,TConnectionProperties> BotEventEntersWorld;
        /// <summary>
        /// Occurs when [bot logged in].
        /// </summary>
        event BotEventLoggedInDelegate<TSender, TConnectionProperties> BotEventLoggedIn;
        /// <summary>
        /// Occurs when [avatar clicks on an object].
        /// </summary>

    }
}
