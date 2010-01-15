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
using AwManaged.Core.Interfaces;
using AwManaged.Core.ServicesManaging.Interfaces;

namespace AwManaged.Core.EventHandling
{
    [Serializable]
    public delegate void ServiceStartedelegate(IServicesManager sender, ServiceStartedArgs e);

    public sealed class ServiceStartedArgs : MarshalByRefObject
    {
        private readonly IIdentifiable _service;

        public IIdentifiable Service
        {
            get { return _service; }
        }

        public ServiceStartedArgs(IIdentifiable service)
        {
            _service = service; // todo, maybe make this a clone, so we can't actually stop a service from the custom bot.
        }
    }
}