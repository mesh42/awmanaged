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
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Serialization;
using AW;
using AwManaged.Converters;
using AwManaged.Core;
using AwManaged.Math;
using AwManaged.Scene;
using NUnit.Framework;
using Zone=AwManaged.Scene.Zone;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class ObjectTests
    {



        [Test]
        public void SimpleDeltaTest()
        {
            var model1 = new Model() { Action = "hello world!", Description = "description" };
            var model2 = new Model() { Action = "hello mars!", Description = "description" };

            var diff = Differential.Properties(model1, model2);
        }

        [Test]
        public void ReadOnlyCollection()
        {
            var l = new System.Collections.Generic.List<Model>();
            l.Add(new Model(){Action = "bla"});
            var r = l.AsQueryable();
            var s = r.Single(p => p.Action == "bla");
            s.ModelName = "Yea";
        }

        [Test]
        public void ProtectedListTests()
        {
            try
            {
                var a = new ProtectedList<Model> {new Model {Description = "description", Action = "hello"}};
                var i = a.Count;

                foreach (var b in from p in a select p)
                {
                }
            }
            catch (SecurityException ex) { }
        }

        [Test]
        public void CoordinateConversionTests()
        {
            var result = AwConvert.CoordinatesToVector3("5.73S 1.83E 0.1a 221");
            var result2 = AwConvert.Vector3ToCoordinates(result.Position, result.Rotation.y);
        }

        [Test]
        public void ObjectSerializationTests()
        {
            var o = new Model(1, 1, DateTime.Now, ObjectType.V3, "model1", new Vector3(10, 20, 30),
                              new Vector3(40, 50, 60), "description", "action"/*, 10, string.Empty*/);
            Serialize(o, "test.xml");
        }

        [Test]
        public void ZoneSerializationTests()
        {
            var model = new Model(1, 1, DateTime.Now, ObjectType.V3, "model1", new Vector3(10, 20, 30),
                                  new Vector3(40, 50, 60), "description", "action"/*, 10, string.Empty*/);

            var zoneV4 = new Zone();

            Serialize(zoneV4, "ZoneObject.xml");
        }

        private static void Serialize(object o, string path)
        {
            var x = new XmlSerializer(o.GetType());
            var sw = new StreamWriter(path);
            x.Serialize(sw, o);
        }
    }
}