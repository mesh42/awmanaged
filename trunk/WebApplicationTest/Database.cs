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
using AwManaged.Core.ServicesManaging;
using AwManaged.Storage;

namespace WebApplicationTest
{
    public static class Database
    {
        public static Db4OStorageClient Storage;
        public static Db4OStorageServer Server;
        public static ServicesManager svc;

        static Database()
        {
            Server = new Db4OStorageServer("provider=db4o;user=awmanaged;password=awmanaged;port=4572;file=performancetest.dat") { IdentifyableTechnicalName = "server" };
            Storage = new Db4OStorageClient("provider=db4o;user=awmanaged;password=awmanaged;port=4572;server=localhost") { IdentifyableTechnicalName = "client" };
            var svc = new ServicesManager();
            svc.Start();
            svc.AddService(Server);
            svc.AddService(Storage);
            svc.StartServices();
        }
    }
}
