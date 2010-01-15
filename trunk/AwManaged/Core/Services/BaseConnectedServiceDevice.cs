﻿using System;
using AwManaged.Core.Interfaces;

namespace AwManaged.Core.Services
{
    public abstract class BaseConnectedServiceDevice<TConnectionInterface> : BaseService, IConnectedServiceDevice<TConnectionInterface>
        where TConnectionInterface : BaseConnection<TConnectionInterface>, new()
    {
        protected BaseConnectedServiceDevice(string connection)
        {
            Connection = new TConnectionInterface {ConnectionString = connection};
        }

        #region IConnectedServiceDevice<TConnectionInterface> Members

        public abstract string ProviderName { get; }
        internal abstract void EvaluateConnectionProperties();

        internal void ThrowIncorrectConnectionStringException()
        {
            throw new ArgumentException("Connectionstring is in the incorrect format");
        }

        #endregion

        #region IConnectionProperties<TConnectionInterface> Members

        public TConnectionInterface Connection { get; internal set; }

        #endregion
    }
}
