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
using AwManaged.Core.Services.Interfaces;

namespace AwManaged.Core.Services
{
    public abstract class BaseService : MarshalIndefinite, IService
    {
        #region IService Members

        public virtual bool Stop()
        {
            if (!IsRunning)
                throw new Exception(string.Format("The {0} service can't stop, it is not running.", IdentifyableDisplayName));
            IsRunning = false;
            return true;
        }

        public virtual bool Start()
        {
            if (IsRunning)
                throw new Exception(string.Format("The {0} service can't start, it is already started.", IdentifyableDisplayName));
            IsRunning = true;
            return true;
        }

        public virtual bool IsRunning
        {
            get;
            private set;
        }

        #endregion

        #region IIdentifiable Members

        public abstract string IdentifyableDisplayName {get;}

        public abstract Guid IdentifyableId { get;}

        public virtual string IdentifyableTechnicalName { get; set; }

        #endregion

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
