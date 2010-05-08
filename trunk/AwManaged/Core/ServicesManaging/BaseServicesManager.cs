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
using System.Collections.Generic;
using System.Resources;
using AwManaged.Core.EventHandling;
using AwManaged.Core.Services.Interfaces;
using AwManaged.Core.ServicesManaging.Interfaces;
using AwManaged.Properties;

namespace AwManaged.Core.ServicesManaging
{
    public abstract class BaseServicesManager : IServicesManager
    {
        private List<IService> _services;

        protected BaseServicesManager(){}

        #region IServicesManager Members

        public void AddService(IService service)
        {
            if (!IsRunning)
                throw new Exception(string.Format(Resources.services_manager_not_running, IdentifyableTechnicalName));
            if (string.IsNullOrEmpty(service.IdentifyableTechnicalName))
                throw new Exception(Resources.err7_service_error_technical_name);
            var svc = _services.Find(p => p.IdentifyableTechnicalName == service.IdentifyableTechnicalName);
            if (svc != null)
                throw new Exception(string.Format("Could not add the {0} Service to the service manager. A service with an identical technical name already exists.", service.IdentifyableTechnicalName));
            _services.Add(service);
        }

        public void RemoveService(string technicalName)
        {
            if (!IsRunning)
                throw new Exception(string.Format("Services manager '{0}' not running.", IdentifyableTechnicalName));
            if (String.IsNullOrEmpty(technicalName))
                throw new Exception("Please provide a technical valid service name.");
            var service = _services.Find(p => p.IdentifyableTechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.IdentifyableTechnicalName));
            if (service.IsRunning)
                throw new Exception(string.Format("Could not remove the {0} Service, as its currently running please stop the service first.", service.IdentifyableTechnicalName));
            _services.RemoveAll(p => p.IdentifyableTechnicalName == technicalName);
        }

        public virtual IService StartService(string technicalName)
        {
            if (!IsRunning)
                throw new Exception(string.Format(Resources.services_manager_not_running, IdentifyableTechnicalName));
            var service = _services.Find(p => p.IdentifyableTechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.IdentifyableTechnicalName));
            if (service.IsRunning)
                throw new Exception(string.Format("Could not start the {0} Service, as its already running.", service.IdentifyableTechnicalName));
            if (!service.Start())
                throw new Exception(string.Format("Could not start the {0} Service, because of its internal state.", service.IdentifyableTechnicalName));
            return service;
        }

        public virtual void StopService(string technicalName)
        {
            if (!IsRunning)
                throw new Exception(string.Format("Services manager '{0}' not running.", IdentifyableTechnicalName));
            var service = _services.Find(p => p.IdentifyableTechnicalName == technicalName);
            if (service == null)
                throw new Exception(string.Format("Could not find the {0} Service in the service manager.", service.IdentifyableTechnicalName));
            if (!service.IsRunning)
                throw new Exception(string.Format("Could not stop the {0} Service, as its currently not running.", service.IdentifyableTechnicalName));
            if (!service.Stop())
                throw new Exception(string.Format("Could not stop the {0} Service, because of its internal state.", service.IdentifyableTechnicalName));
        }

        #endregion

        #region IServicesManager Members

        public IList<IService> List()
        {
            return _services;
        }

        public event AwManaged.Core.EventHandling.ServiceStartedelegate OnServiceStarted;
        public event AwManaged.Core.EventHandling.ServiceStartedelegate OnServiceManagerStarted;

        #endregion

        #region IService Members

        /// <summary>
        /// Stops all the services managed by this service manager instance in the reverse order the where started.
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            if (!IsRunning)
                throw new Exception(string.Format("Services manager '{0}' not running.", IdentifyableTechnicalName));
            for (var i = _services.Count-1;i>-1;i--)
            {
                if (!_services[i].IsRunning)
                    throw new Exception(string.Format("Can't stop the {0} Service, service is not running.", _services[i].IdentifyableTechnicalName));
                _services[i].Stop();
            }
            _services.Clear();
            _services = null;
            IsRunning = false;
            return true;
        }

        /// <summary>
        /// Starts all the services managed by this service manager instance in the order they where added
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            if (IsRunning)
                throw new Exception(string.Format("Services manager '{0}' already running.",IdentifyableTechnicalName));
            _services = new List<IService>();
            IsRunning = true;
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
            get; internal set;
        }

        #endregion

        #region IIdentifiable Members

        public virtual string IdentifyableDisplayName { get; set; }

        public virtual Guid IdentifyableId { get; set; }

        public virtual string IdentifyableTechnicalName { get; set; }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// This causes a hard forced shutdown of all services.
        /// </summary>
        public void Dispose()
        {
            if (_services != null)
            {
                foreach (var service in _services)
                    service.Dispose();
                _services.Clear();
            }
        }


        #endregion

        #region IServicesManager Members

        public virtual bool StartServices()
        {
            if (!IsRunning)
                throw new Exception(string.Format("Services manager '{0}' is not running.", IdentifyableTechnicalName));
            foreach (var service in _services)
            {
                if (service.IsRunning)
                    throw new Exception(string.Format("Can't start the {0} Service, service is already running.", service.IdentifyableTechnicalName));
                service.Start();
                if (OnServiceStarted != null)
                    OnServiceStarted.Invoke(this, new ServiceStartedArgs(service));
            }
            return true;
        }

        #endregion
    }
}