using System;
using AwManaged.Core.Services.Interfaces;

namespace AwManaged.Core.Services
{
    public abstract class BaseService : MarshalByRefObject, IService
    {
        #region IService Members

        public virtual bool Stop()
        {
            if (!IsRunning)
                throw new Exception(string.Format("The {0} service can't stop, it is not running.", DisplayName));
            IsRunning = false;
            return true;
        }

        public virtual bool Start()
        {
            if (IsRunning)
                throw new Exception(string.Format("The {0} service can't start, it is already started.", DisplayName));
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

        public abstract string DisplayName {get;}

        public abstract Guid Id { get;}

        public abstract string TechnicalName { get;}

        #endregion

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
