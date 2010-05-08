using System;
using SharedMemory;using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace AwManaged.RemoteServices
{
    public class CrossAppDomainSingletonT<T> : MarshalIndefinite where T : new()
    {
        private static readonly string AppDomainName = "Singleton AppDomain";
        private static T _instance;

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

        private static AppDomain GetAppDomain(string friendlyName)
        {
            IntPtr enumHandle = IntPtr.Zero;
            var host = new mscoree.CorRuntimeHostClass(); 
            try
            {
                host.EnumDomains(out enumHandle);

                object domain = null;
                while (true)
                {
                    host.NextDomain(enumHandle, out domain);
                    if (domain == null)
                    {
                        break;
                    }
                    AppDomain appDomain = (AppDomain)domain;
                    if (appDomain.FriendlyName.Equals(friendlyName))
                    {
                        return appDomain;
                    }
                }
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
                host = null;
            }
            return null;
        }


        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    AppDomain appDomain = GetAppDomain(AppDomainName);
                    if (null == appDomain)
                    {
                        appDomain = AppDomain.CreateDomain(AppDomainName);
                    }
                    Type type = typeof(T);
                    T instance = (T)appDomain.GetData(type.FullName);
                    if (null == instance)
                    {
                        instance = (T)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                        appDomain.SetData(type.FullName, instance);
                    }
                    _instance = instance;
                }

                return _instance;
            }
        }
    }
}
