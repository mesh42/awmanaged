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
using System.Reflection;
using AwManaged.Core.Commanding;
using AwManaged.Core.Commanding.Attributes;
using AwManaged.Core.ServicesManaging;
using AwManaged.Scene.ActionInterpreter;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Storage;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{

    [TestFixture]
    public sealed class ActionInterpreterTests 
    {
        private GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute, CCItemBindingAttribute> _ccSvc;
        private GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute> _acSvc;

        [TearDown]
        public void TearDown()
        {
            _ccSvc.Dispose();
            _acSvc.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _ccSvc = new GenericInterpreterService<CCEnumTypeAttribute, CCEnumBindingAttribute, CCItemBindingAttribute>(Assembly.GetAssembly(typeof(ServerConsole)));
            _ccSvc.Start();
            _acSvc = new GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute>(Assembly.GetAssembly(typeof(ACEnumBindingAttribute)));
            _acSvc.Start();
        }

        private string ToActionLiteralPart(object o)
        {
            string literal = string.Empty;
            foreach (var prop in o.GetType().GetProperties())
            {
                var att = prop.GetCustomAttributes(typeof(ACItemBindingAttribute), false);
                if (att !=null && att.Length == 1)
                {
                    var castAtt = (ACItemBindingAttribute) att[0];
                    var value = prop.GetValue(o, null);
                    if (value==null)
                        continue;

                    switch (castAtt.Type)
                    {
                        case CommandInterpretType.Flag:
                            literal += castAtt.LiteralName + " ";
                            break;
                        case CommandInterpretType.NameValuePairs:
                            if (value.GetType() == typeof(string))
                            {
                                literal += castAtt.LiteralName + "=" + (string) value + " ";
                                break;    
                            }
                            if (value.GetType() == typeof(int))
                            {
                                literal += castAtt.LiteralName + "=" + (int)value + " ";
                                break;
                            }
                            if (value.GetType().IsEnum)
                            {
                                literal += castAtt.LiteralName + "=" + (int)value + " ";
                            }
                            //else
                            //{
                                
                            //}
                            //break;
                            break;
                    }
                    
                }
            }
            return literal.Trim();
        }

        [Test]
        public void SaveActionObjectTest()
        {
            var server = new Db4OStorageServer("provider=db4o;user=awmanaged;password=awmanaged;port=4573;file=actionstorage.dat") { IdentifyableTechnicalName = "server" };
            server.Start();
            var client = new Db4OStorageClient("provider=db4o;user=awmanaged;password=awmanaged;port=4573;server=localhost") { IdentifyableTechnicalName = "client" };
            client.Start();
            var fx = new ACMatFx();
            fx.Blend = TextureBlendType.SourceAlphaSaturation;
            fx.Type = MatFxType.DualTexturingCamera;
            ToActionLiteralPart(fx);
            client.Db.Store(fx);
            client.Db.Commit();
            client.Stop();
            server.Stop();
        }

        [Test]
        public void LoadPluginTest()
        {
            _ccSvc.Interpret("load plugin gbot /persist.");

        }

        [Test]
        public void ActionResolveTest()
        {
            var ret = _acSvc.Interpret("activate media set vol=100 osd=on name=foo");
        }
    }
}
