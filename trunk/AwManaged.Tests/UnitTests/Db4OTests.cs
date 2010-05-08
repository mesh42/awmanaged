using SharedMemory;using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AwManaged.Core.ServicesManaging;
using AwManaged.Storage;
using Db4objects.Db4o;
using Db4objects.Db4o.Config.Attributes;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    public class TestObject
    {
        [Indexed]
        private int _id;
        [Indexed]
        private DateTime _start;
        [Indexed]
        public DateTime _end;
        [Indexed]
        private string _name;

        public TestObject(){}

        public TestObject(int id, DateTime start, DateTime end, string name)
        {
            _id = id;
            _start = start;
            _end = end;
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public DateTime End
        {
            get { return _end; }
        }

        public DateTime Start
        {
            get { return _start; }
        }

        public int Id
        {
            get { return _id; }
        }
    }



    [TestFixture]
    public class Db4OTests
    {
        private Db4OStorageServer server;
        private Db4OStorageClient client;
        private ServicesManager svc;

        [SetUp]
        public void SetUp()
        {
            server = new Db4OStorageServer("provider=db4o;user=awmanaged;password=awmanaged;port=4572;file=performancetest.dat") { IdentifyableTechnicalName = "server" };
            client = new Db4OStorageClient("provider=db4o;user=awmanaged;password=awmanaged;port=4572;server=localhost") { IdentifyableTechnicalName = "client" };
            svc = new ServicesManager();
            svc.Start();
            svc.AddService(server);
            svc.AddService(client);
            svc.StartServices();
        }

        private void BuildGrid<T>(PropertyInfo t) where T : new()
        {
            client.Db.Ext().Configure().ObjectClass(typeof(TestObject)).ObjectField("_name").Indexed(true);
            client.Db.Ext().Configure().OptimizeNativeQueries(true);
            client.Db.Ext().Configure().WeakReferences(true);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //var q = client.Db.Query();
            //q.Constrain(typeof(TestObject));
            //q.Descend("_name").Equals("User50000");
            //var e = q.Execute();
            //var o = e.Next();

            var q = client.Db.Query<TestObject>().Where(p => p.Name == "User50000").GetEnumerator();
            q.MoveNext();
            var o = q.Current;


            



           // var result = q.Select(p => p.Name == "User50000").ToList();
            //var r = q.OrderBy(p => p.End).Skip(5000).Take(5).ToList();
            

            //sw.Start();
            //var q = client.Db.Query();
            //q.Constrain(new T());
            //q.Descend("_id").
            //var r = q.Execute();
            //var o = r.Next();

            //q.Constrain(new T());
            //q.Descend("_name").Constrain("User50000");
            //var r =q.Execute();

            ////q.Constrain("_end").
            //var q = from TestObject p in client.Db where p.Name == "User50000" select p;
            //var result = q.Execute();
            //var o = result.Next();
            //var r = q.First();
            //var set = q.Execute();
            //for (int i=0;i<10000;i++)
            //{
            //    set.
            //}

          //  var q = client.Db.Query<TestObject>(p => p.Name == "User50000");



            //int c = q.Count;
            //var s = q.Single();
            sw.Stop();
            long elapsed = sw.ElapsedMilliseconds;


        }

        [Test]
        public void Query()
        {
            Type t = typeof (TestObject);
            BuildGrid<TestObject>(t.GetProperty("_end"));

        }

        [Test] void LocalTest()
        {
        }

        [Test]
        public void QueryTest()
        {
            client.Db.Ext().Configure().ObjectClass(typeof(TestObject)).ObjectField("_name").Indexed(true);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 500000; i++)
            {
                client.Db.Store(new TestObject(i, DateTime.Now, DateTime.Now, "User" + i));
            }
            client.Db.Commit();
            sw.Stop();
            long elapsed = sw.ElapsedMilliseconds;
        }

        [TearDown]
        public void TearDown()
        {
            svc.Stop();
        }
    }
}
