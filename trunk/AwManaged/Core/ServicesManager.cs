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
using System.Collections.Generic;
using AwManaged.Core.EventHandling;
using AwManaged.Core.Interfaces;

namespace AwManaged.Core
{
    /// <summary>
    /// Contains services.
    /// </summary>
    public class ServicesManager : IServicesManager
    {
        private readonly List<IService> _services;

        public ServicesManager()
        {
            _services = new List<IService>();
        }
        


        #region IService Members

        /// <summary>
        /// Stops all the services managed by this service manager instance in the reverse order the where started.
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            foreach (var service in _services)
            {
                if (!service.IsRunning)
                    throw new Exception(string.Format("Can't stop the {0} Service, service is not running.", service.TechnicalName));
                service.Stop();
            }
            return true;
        }

        /// <summary>
        /// Starts all the services managed by this service manager instance in the order they where added
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            foreach (var service in _services)
            {
                if (service.IsRunning)
                    throw new Exception(string.Format("Can't start the {0} Service, service is already running.",service.TechnicalName));
                service.Start();
                if (OnServiceStarted != null)
                    OnServiceStarted.Invoke(this, new ServiceStartedArgs(service));
            }
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// The service manager is running upon initialization.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return true; }
        }

        #endregion

        #region IServicesManager Members

        public void AddService(IService service)
        {
            if (service == null)
                throw new Exception("Service is null");
            if (string.IsNullOrEmpty(service.TechnicalName))
                throw new Exception("A service must have a technical name in order to run it in the service manager.");
            var svc = _services.Find(p => p.TechnicalName == service.TechnicalName);
            if (svc != null)
                throw new Exception(string.Format("Could not add the {0} Service to the service manager. A service with an identical technical name already exists.", service.TechnicalName));
            _services.Add(service);
        }

        public void RemoveService(string technicalName)
        {
            if (String.IsNullOrEmpty(technicalName))
                throw new Exception("Please provide a technical valid service name.");
            var service = _services.Find(p => p.TechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.TechnicalName));
            if (service.IsRunning)
                throw new Exception(string.Format("Could not remove the {0} Service, as its currently running please stop the service first.", service.TechnicalName));
            service.Dispose();
            _services.RemoveAll(p => p.TechnicalName == technicalName);
        }

        public void StartService(string technicalName)
        {
            var service = _services.Find(p => p.TechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.TechnicalName));
            if (service.IsRunning)
                throw new Exception(string.Format("Could not start the {0} Service, as its already running.", service.TechnicalName));
        }

        public void StopService(string technicalName)
        {
            var service = _services.Find(p => p.TechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.TechnicalName));
            if (!service.IsRunning)
                throw new Exception(string.Format("Could not stop the {0} Service, as its currently not running.", service.TechnicalName));
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// This causes a hard forced shutdown of all services.
        /// </summary>
        public void Dispose()
        {
            foreach (var service in _services)
                service.Dispose();
            _services.Clear();
        }

        #endregion

        #region IIdentifiable Members

        public string DisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public string TechnicalName
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IServicesManager Members


        public event AwManaged.Core.EventHandling.ServiceStartedelegate OnServiceStarted;

        #endregion

        #region IServicesManager Members

        public IList<IService> List()
        {
            return _services;
        }

        #endregion
    }
}
