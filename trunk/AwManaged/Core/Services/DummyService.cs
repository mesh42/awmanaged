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
using AwManaged.Core.Services.Interfaces;

namespace AwManaged.Core.Services
{
    public class DummyService : IService
    {
        #region IService Members

        public bool Stop()
        {
            if (!IsRunning)
                throw new Exception();
            IsRunning = false;
            return true;
        }

        public bool Start()
        {
            if (IsRunning)
                throw new Exception();
            IsRunning = true;
            return true;
        }

        public bool IsRunning
        {
            get; internal set;
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get; set;
        }

        public Guid IdentifyableId
        {
            get; set;
        }

        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}