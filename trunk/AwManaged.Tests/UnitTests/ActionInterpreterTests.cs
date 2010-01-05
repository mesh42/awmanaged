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
using System.Drawing;
using System.Text.RegularExpressions;
using AwManaged.Core.Reflection;
using AwManaged.Scene.ActionInterpreter;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;
using NUnit.Framework;
using System.Linq;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public sealed class ActionInterpreterTests 
    {


        [Test]
        public void ActionResolveTest()
        {
            ReflectionEnumCache enumCache = ReflectionHelpers.GetEnums<ACEnumTypeAttribute, ACEnumBindingAttribute>();

            // perform some linq queries
            var en = from ReflectionEnumCacheItem p in enumCache.Enumerations where p.EnumerationType == typeof(LightType) select p;
            var action = "create light type=spot color=blue name=ceiling pitch=45, corona texture=sunring.jpg;create sign bcolor=red;activate media url=http://212.109.3.19:8000";
            var triggerInterperters = ReflectionHelpers.GetInstancesOfInterface<IActionTrigger>(); // this should be cached once upon startup to increase performance.
            var actionInterpreters = ReflectionHelpers.GetInstancesOfInterface<IActionCommand>(); // this should be cached once upon startup to increase performance.
            var actionTriggers = ReflectionHelpers.Interpret(triggerInterperters, action,';');
            foreach (var actionTrigger in actionTriggers)
            {
                actionTrigger.Commands = ReflectionHelpers.Interpret(actionInterpreters, actionTrigger.LiteralPart, ',');
                foreach (var command in actionTrigger.Commands)
                {
                    // Bind to properties which cary the ACItemBinding Attribute.
                    // todo: put the binding properties in a reflection cache from improved speed.
                    foreach (var property in command.GetType().GetProperties())
                    {
                        var o = property.GetCustomAttributes(typeof (ACItemBindingAttribute), false);
                        if (o != null && o.Length == 1)
                        {
                            var attribute = (ACItemBindingAttribute)o[0];
                            switch (attribute.Type)
                            {
                                case CommandInterpretType.NameValuePairs:
                                    string lowerd = command.LiteralPart.ToLower();
                                    if (lowerd.IndexOf(attribute.LiteralName) > -1)
                                    {

                                        var match =
                                            new Regex(
                                                attribute.LiteralName +
                                                "([\\s]{0,})([=]{1})(?<value>[a-z,A-Z,0-9,.]{1,})([\\s]{1})",
                                                RegexOptions.IgnoreCase).Match(command.LiteralPart);
                                        if (match.Success)
                                        {
                                            if (property.PropertyType == typeof(float))
                                            {
                                                property.SetValue(command, float.Parse(match.Groups["value"].Value), null);
                                                break;
                                            }
                                            if (property.PropertyType == typeof(string))
                                            {
                                                property.SetValue(command, match.Groups["value"].Value, null);
                                                break;
                                            }
                                            if (property.PropertyType.IsEnum)
                                            {
                                                throw new NotImplementedException();
                                            }
                                            if (property.PropertyType == typeof(Color))
                                            {
                                                if (match.Groups["value"].Value.StartsWith("#"))
                                                {
                                                    // interpret the hex raw color.
                                                    property.SetValue(command, ColorTranslator.FromHtml(match.Groups["value"].Value),null);
                                                    break;
                                                }
                                                else
                                                {
                                                    // interpret the color by name. TODO: add color intepretation for specific aw color names.
                                                    property.SetValue(command, Color.FromName(match.Groups["value"].Value), null);
                                                    break;
                                                }
                                                
                                            }
                                        }
                                        else
                                        {
                                            // Mark this command as uninterpretable, so we can't make changes to it using design time components.
                                            var unknownCommand = actionTrigger.Commands.Find(p=> p == command);
                                            unknownCommand = new ACUnknown();
                                        }
                                    }

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                            
                        }
                        //foreach (var attribute in property.Attributes) 
                    }
                }
            }
        }
    }
}
