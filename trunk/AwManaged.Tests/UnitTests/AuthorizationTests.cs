using System.Linq;
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
        public void TestAuthorization()
        {
            var server = new Db4OStorageServer(new StorageConfiguration<Db4OConnection>("provider=db4o;user=awmanaged;password=awmanaged;port=4572;file=authorization.dat"));
            server.Start();
            var client = new Db4OStorageClient(new StorageConfiguration<Db4OConnection>("provider=db4o;user=awmanaged;password=awmanaged;port=4572;server=localhost"));

            var records = from User p in client.Db where p.NameToLower == "unittest" select p;
            User user;
            if (records.Count() == 0)
            {
                user = new User("unittest", "unittest", "unittest@unittest.com");
                var result = user.RegisterUser(client);
                Assert.IsTrue(result == RegistrationResult.Success);
                result = user.RegisterUser(client);
                Assert.IsTrue(result == RegistrationResult.UserExists);
                var user2 = new User("unittest2","unittest", "unittest@unittest.com");
                result = user2.RegisterUser(client);
                Assert.IsTrue(result == RegistrationResult.EmailExists);
            }
            else
            {
                user = records.Single();
                var result = user.RegisterUser(client);
                Assert.IsTrue(result == RegistrationResult.UserExists);
                user = new User("unittest2", "unittest", "unittest@unittest.com");
                result = user.RegisterUser(client);
                Assert.IsTrue(result == RegistrationResult.EmailExists);
                user = records.Single();

            }

            Assert.IsTrue(user.IsAuthenticated(client));
            user = new User("unittest","wrongpass");
            Assert.IsFalse(user.IsAuthenticated(client));
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
            server.Stop();
        }
    }
}
