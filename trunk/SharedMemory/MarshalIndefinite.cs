using System;
using SharedMemory;using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMemory
{
    public abstract class MarshalIndefinite : MarshalByRefObject
    {
        /// <summary>
        /// Override of lifetime service initialization.
        /// The reason for overriding is to have this singleton live forever
        /// </summary>
        /// <returns>object for the lease period - in our case always null</returns>
        public override object InitializeLifetimeService()
        {
            // We want this singleton to live forever
            // In order to have the lease across appdomains live forever,
            // we return null.
            return null;
        }

    }
}

namespace AwManaged.Core
{
    
}