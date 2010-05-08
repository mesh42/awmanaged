using SharedMemory;using System;

namespace AwManaged.RemoteServices
{
    public class CrossAppDomainSingleton : CrossAppDomainSingletonT<CrossAppDomainSingleton>
    {
        /// <summary>
        /// Override of lifetime service initialization.
        /// 
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

        public BotEngine Shared;

        //public void HelloWorld()
        //{
        //    Console.WriteLine("Hello world from '" + AppDomain.CurrentDomain.FriendlyName + " (" + AppDomain.CurrentDomain.Id + ")'");
        //}
    }
}
