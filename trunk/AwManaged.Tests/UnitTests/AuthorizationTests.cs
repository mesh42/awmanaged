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
using System.Diagnostics;
using System.Linq;
using AwManaged.Core.ServicesManaging;
using AwManaged.Security.RemoteBotEngine;
using AwManaged.Storage;
using Db4objects.Db4o.Linq;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture()]
    public class AuthorizationTests
    {
        [Test]
        public void TestPerformance()
        {
            var server = new Db4OStorageServer("provider=db4o;user=awmanaged;password=awmanaged;port=4572;file=performancetest.dat") { TechnicalName = "server" };
            var client = new Db4OStorageClient("provider=db4o;user=awmanaged;password=awmanaged;port=4572;server=localhost") { TechnicalName = "client" };
            var svc = new ServicesManager();
            svc.Start();
            svc.AddService(server);
            svc.AddService(client);
            svc.StartServices();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000;i++ )
            {
                User user = new User("User"+i,"Password","Test@Test.com");
                client.Db.Store(user);
            }
            client.Db.Commit();
            sw.Stop();
            svc.Stop();
        }


        [Test]
        public void TestAuthorization()
        {
            var server =new Db4OStorageServer("provider=db4o;user=awmanaged;password=awmanaged;port=4572;file=authorization.dat"){TechnicalName = "server"};
            var client =new Db4OStorageClient("provider=db4o;user=awmanaged;password=awmanaged;port=4572;server=localhost") {TechnicalName = "client"};
            var svc = new ServicesManager();
            svc.Start();
            svc.AddService(server);
            svc.AddService(client);
            svc.Start();

            var records = from User p in client.Db where p.NameToLower == "unittest" select p;
            User user;
            if (records.Count() == 0)
            {
                user = new User("unittest", "unittest", "unittest@unittest.com");
                var result = user.RegisterUser(client.Db);
                Assert.IsTrue(result == RegistrationResult.Success);
                result = user.RegisterUser(client.Db);
                Assert.IsTrue(result == RegistrationResult.UserExists);
                var user2 = new User("unittest2","unittest", "unittest@unittest.com");
                result = user2.RegisterUser(client.Db);
                Assert.IsTrue(result == RegistrationResult.EmailExists);
            }
            else
            {
                user = records.Single();
                var result = user.RegisterUser(client.Db);
                Assert.IsTrue(result == RegistrationResult.UserExists);
                user = new User("unittest2", "unittest", "unittest@unittest.com");
                result = user.RegisterUser(client.Db);
                Assert.IsTrue(result == RegistrationResult.EmailExists);
                user = records.Single();

            }

            Assert.IsTrue(user.IsAuthenticated(client.Db));
            user = new User("unittest","wrongpass");
            Assert.IsFalse(user.IsAuthenticated(client.Db));
            records = from User p in client.Db where p.NameToLower == "unittest" select p;
            user = records.Single();            

            var auth = new Authorization<User>(client.Db, user, "administrator");
            auth.AddAuthorization();
            Assert.IsTrue(auth.HasAuthorization());
            auth.RemoveAuthorization();
            auth = new Authorization<User>(client.Db,user, "backup-operator");
            Assert.IsFalse(auth.HasAuthorization());
            auth.AddAuthorization();
            Assert.IsTrue(auth.HasAuthorization());
            var roles = Authorization<User>.GetAuthorizations(client, user);
            Assert.IsTrue(roles.Count == 1 && roles[0] == "backup-operator");
            auth.RemoveAuthorization();

            svc.Stop();
        }
    }
}
