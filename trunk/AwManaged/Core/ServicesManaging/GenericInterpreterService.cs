﻿/* **********************************************************************************
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AwManaged.Core.Commanding;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Reflection;
using AwManaged.Core.Services.Interfaces;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Core.ServicesManaging
{
    public class GenericInterpreterService<TEnumTypeAttribute, TEnumBindingAttribute, TItemBindingAttribute> : IService
        where TEnumBindingAttribute : Attribute, IACLiteralNames
        where TEnumTypeAttribute: Attribute
        where TItemBindingAttribute : Attribute, ICommandItemBindingAttribute

    {
        private readonly Assembly _assembly;

        public ReflectionCache<TEnumTypeAttribute, TEnumBindingAttribute> Cache;



        public GenericInterpreterService(Assembly assembly)
        {
            _assembly = assembly;
            Cache = new ReflectionCache<TEnumTypeAttribute, TEnumBindingAttribute>(assembly);
        }

        //ACEnumTypeAttribute, ACEnumBindingAttribute
        public Assembly Assembly
        {
            get { return _assembly; }
        }

        /// <summary>
        /// Casts the action string command property value to a action command property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="uncastedValue">The uncasted value.</param>
        /// <returns>false if the cast was not successfull</returns>
        private bool CastValueToProperty(Object instance, PropertyInfo property, TItemBindingAttribute attribute, string uncastedValue)
        {
            if (attribute.Delimiter != 0)
            {
                // multivalued.
                var b = property.PropertyType.GetGenericArguments();
                switch (b[0].FullName) //todo: cast this to an actual type. instead of string.
                {
                    case "System.Int32" :
                        var list = new List<System.Int32>(); // make this more generic, see todo.

                        foreach (string item in uncastedValue.Split(attribute.Delimiter))
                        {
                            list.Add(int.Parse(item));
                        }
                        property.SetValue(instance, list, null);
                        return true;

                }
            }

            // bool flag.
            if (property.PropertyType == typeof(bool))
            {
                property.SetValue(instance, true, null);
                return true;
            }


            if (property.PropertyType == typeof(int))
            {
                property.SetValue(instance, int.Parse(uncastedValue), null);
                return true;
            }
            if (property.PropertyType == typeof(float))
            {
                property.SetValue(instance, float.Parse(uncastedValue), null);
                return true;
            }
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(instance, uncastedValue, null);
                return true;
            }
            if (property.PropertyType.IsEnum)
            {
                var e = from ReflectionEnumCacheItem p in Cache.EnumCache.Enumerations
                        where p.EnumerationType == property.PropertyType
                        select p;
                if (e.Count() == 1)
                {
                    var f = e.Single().GetFieldByLiteralName(uncastedValue);
                    property.SetValue(instance, f.Value, null);
                    return true;
                }
            }
            if (property.PropertyType == typeof(Color))
            {
                if (uncastedValue.StartsWith("#"))
                {
                    // interpret the hex raw color.
                    property.SetValue(instance, ColorTranslator.FromHtml(uncastedValue), null);
                    return true;
                }
                else
                {
                    // interpret the color by name. TODO: add color intepretation for specific aw color names.
                    property.SetValue(instance, Color.FromName(uncastedValue), null);
                    return true;
                }

            }
            if (property.PropertyType == typeof(byte))
            {
                property.SetValue(instance, byte.Parse(uncastedValue), null);
            }
            return false;
        }

        /// <summary>
        /// Gets the a regex match for the current command property interpretation type.
        /// </summary>
        /// <param name="literalCommandPart">The literal command part.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private Match GetMatchForCommandInterpretType(string literalCommandPart, TItemBindingAttribute attribute, PropertyInfo property)
        {
            switch (attribute.Type)
            {
                case CommandInterpretType.SingleArgument:
                    return new Regex("\\s(?<value>" + literalCommandPart + ")\\s",
                                     RegexOptions.IgnoreCase).Match(" " + literalCommandPart + " ");
                case CommandInterpretType.FlagSlash:
                    return new Regex("\\s/(?<value>" + attribute.LiteralName + ")\\s",
                                     RegexOptions.IgnoreCase).Match(" " + literalCommandPart + " ");
                case CommandInterpretType.Flag:
                    var en = from ReflectionEnumCacheItem p in Cache.EnumCache.Enumerations where p.EnumerationType == property.PropertyType select p;
                    foreach (var field in en.Single().ItemFields)
                    {
                        foreach (var name in field.LiteralNames)
                        {
                            var match = new Regex("\\s(?<value>" + name + ")\\s",
                                                  RegexOptions.IgnoreCase).Match(" " + literalCommandPart + " ");
                            if (match.Success)
                                return match;
                        }
                    }
                    return null;
            }

            if (literalCommandPart.ToLower().IndexOf(attribute.LiteralName) > -1)
            {
                switch (attribute.Type)
                {
                    case CommandInterpretType.NameValuePairs:
                        return
                            new Regex(attribute.LiteralName + "([\\s]{0,})([=]{1})(?<value>[a-z,A-Z,0-9,.,:]{1,})([\\s]{1})",
                                      RegexOptions.IgnoreCase).Match(literalCommandPart + " ");
                    case CommandInterpretType.NameValuePairsSpace:
                        return
                            new Regex(
                                attribute.LiteralName + "([\\s]{0,})([\\s]{1,})(?<value>[a-z,A-Z,0-9,.]{1,})([\\s]{1})",
                                RegexOptions.IgnoreCase).Match(literalCommandPart + " ");
                    default:
                        throw new Exception(string.Format("Command interpretation not supported for {0}", attribute.Type));
                }
            }
            return null;
        }

        /// <summary>
        /// Inteprets the specified action string, and returns a collection of ActionTrigger with for each trigger, 
        /// the command sets and their object types containing the reflected .NET runtime properties.
        /// 
        /// if the action string can't be intepreted, null will be returned.
        /// todo, implement partial interpretation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public IEnumerable<ICommandGroup> Interpret(string action)
        {
            //try
            //{
           if (!IsRunning)
                throw new Exception(string.Format("Service {0} can't interpret action string. The service is not running.", IdentifyableDisplayName));
 
                var actionTriggers = ReflectionHelpers.Interpret(Cache.TriggerInterpreters, action, ';');
                foreach (var actionTrigger in actionTriggers)
                {
                    actionTrigger.Commands = ReflectionHelpers.Interpret(Cache.CommandInterpreters,
                                                                         actionTrigger.LiteralPart, ',');
                    foreach (var command in actionTrigger.Commands)
                    {
                        var tempLiteral = command.LiteralPart;
                        // Bind to properties which cary the ACItemBinding Attribute.
                        // todo: put the binding properties in a reflection cache from improved speed.
                        foreach (var property in command.GetType().GetProperties())
                        {
                            // quick an dirty bind to the output of this function.
                            if (property.PropertyType == typeof(List<ICommandGroup>))
                            {
                                property.SetValue(command,actionTriggers,null);
                                continue;
                            }

                            var o = property.GetCustomAttributes(typeof (TItemBindingAttribute), false);
                            if (o != null && o.Length == 1)
                            {
                                var attribute = (TItemBindingAttribute) o[0];
                                Match match = null;
                                try
                                {
                                    match = GetMatchForCommandInterpretType(tempLiteral, attribute, property);
                                }
                                catch
                                {

                                }
                                if (match != null && match.Success)
                                {
                                    try
                                    {
                                        tempLiteral = tempLiteral.Replace(match.Value.Trim(), "");
                                    }
                                    catch
                                    {
                                        
                                    }
                                    CastValueToProperty(command, property, attribute, match.Groups["value"].Value.Trim());
                                }
                            }
                            //foreach (var attribute in property.Attributes) 
                        }
                    }
                }
                return actionTriggers;
            //}
            //catch (Exception ex)
            //{
            //    return null; // TODO: implement partial interpretation.
            //}
        }

        #region IService Members

        public bool Stop()
        {
            if (!IsRunning)
                throw new Exception(string.Format("The {0} can't stop, it is not running.", IdentifyableDisplayName));
            Cache = null;
            IsRunning = false;
            return true;
        }

        public bool Start()
        {
            if (IsRunning)
                throw new Exception(string.Format("The {0} can't start, is already started.",IdentifyableDisplayName));
            Cache = new ReflectionCache<TEnumTypeAttribute, TEnumBindingAttribute>(_assembly);
            IsRunning = true;
            return true;
        }

        public bool IsRunning
        {
            get; private set;
        }

        #endregion

        #region IIdentifiable Members

        public string IdentifyableDisplayName
        {
            get { return "Generic Interpreter Service"; }
        }

        public System.Guid IdentifyableId
        {
            get { throw new System.NotImplementedException(); }
        }

        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}