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
using AwManaged.Core.ServicesManaging;
using AwManaged.Scene.ActionInterpreter.Attributes;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{

    [TestFixture]
    public sealed class ActionInterpreterTests 
    {

        [Test]
        public void ActionResolveTest()
        {
            var svc = new GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute,ACItemBindingAttribute>(Assembly.GetAssembly(typeof(ACEnumBindingAttribute)));
            svc.Start();

           // var ret = svc.Interpret("activate envi res=512 type=1 upd=25 time=20 aspect=0.5 zoom=0.8, @key value=mykey");
            //var ret = svc.Interpret("bump lock owners=4711:1174:333333");
            //var ret = svc.Interpret("create matfx type=2 coef=1 tex=self");
            var ret = svc.Interpret("activate media set vol=100 osd=on name=foo");
        }
    }
}
