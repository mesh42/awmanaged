﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AW;
using AwManaged.Converters;
using AwManaged.Core;
using AwManaged.Math;
using AwManaged.SceneNodes;
using NUnit.Framework;

namespace AwManaged.Tests
{
    [TestFixture]
    public class ObjectTests
    {
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
            var a = new ProtectedList<Model> {new Model {Id = 5, Action = "hello"}};
            var i = a.Count;

            foreach (var b in from p in a select p)
            {
                
            }
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
                              new Vector3(40, 50, 60), "description", "action", 10, string.Empty);
            Serialize(o, "test.xml");
        }

        [Test]
        public void ZoneSerializationTests()
        {
            var model = new Model(1, 1, DateTime.Now, ObjectType.V3, "model1", new Vector3(10, 20, 30),
                                  new Vector3(40, 50, 60), "description", "action", 10, string.Empty);

            var zoneV4 = new SceneNodes.Zone();

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