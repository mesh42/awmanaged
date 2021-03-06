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
using System.Collections.Generic;
using AwManaged.Core.Services.Interfaces;

namespace AwManaged.Core.ServicesManaging.Interfaces
{
    public interface IServicesManager : IService
    {
        bool StartServices();
        void AddService(IService service);
        void RemoveService(string technicalName);
        IService StartService(string technicalName);
        void StopService(string technicalName);
        IList<IService> List();
        /// <summary>
        /// Occurs when a service has been started by the event manager.
        /// </summary>
        event AwManaged.Core.EventHandling.ServiceStartedelegate OnServiceStarted;
    }
}