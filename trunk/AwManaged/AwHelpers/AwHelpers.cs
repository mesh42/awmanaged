using System;
using System.Diagnostics;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.ExceptionHandling;

namespace AwManaged.AwHelpers
{
    public static class AwHelpers
    {
        private class ParameterizedConnectThread
        {
            public UniverseConnectionProperties LoginProperties;
            public Instance AwInstance;
        }

        private static void ConnectThread(object data)
        {
            var param = (ParameterizedConnectThread) data;
            param.AwInstance = new Instance(param.LoginProperties.Domain, param.LoginProperties.Port);
        }

        /// <summary>
        /// Create a new AW instance node, by connecting to a universe.
        /// </summary>
        /// <param name="loginProperties">The login properties.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static AW.Instance Connect(UniverseConnectionProperties loginProperties, bool isLbsNode)
        {
            //if (isLbsNode)
            //    return new Instance();

            ParameterizedConnectThread param;
            try
            {
                long _tickStart = DateTime.Now.Ticks;
                var t = new Thread(ConnectThread);
                param = new ParameterizedConnectThread() {LoginProperties = loginProperties};
                t.Start(param);
                var sw = new Stopwatch();
                sw.Start();
                while (param.AwInstance == null)
                {
                    if (sw.ElapsedMilliseconds > loginProperties.ConnectionTimeOut)
                    {
                        t.Abort();
                        throw new NetworkException("Universe connection timed out.");
                    }
                    Thread.Sleep(10);
                }
            }
            catch (InstanceException ex)
            {
                throw new AwException(ex.ErrorCode);
            }
            return param.AwInstance;
        }

        public static void Login(Instance aw, UniverseConnectionProperties loginProperties, bool isLbsNode)
        {
            //if (isLbsNode)
            //    return;
            aw.SetString(Attributes.LoginName, loginProperties.LoginName);
            aw.SetString(Attributes.LoginPrivilegePassword, loginProperties.PrivilegePassword);
            aw.SetInt(Attributes.LoginOwner, loginProperties.Owner);
            var rc = aw.Login();
                if (rc != 0)
                    throw new AwException(rc);
        }

    }
}
