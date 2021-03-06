﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Reflection;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;

namespace AwManaged.Scene.ActionInterpreter
{
    public class ActionInterpreterService : IService
    {
        private ReflectionCache _cache = new ReflectionCache();

        /// <summary>
        /// Casts the action string command property value to a action command property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="uncastedValue">The uncasted value.</param>
        /// <returns>false if the cast was not successfull</returns>
        private bool CastValueToProperty(Object instance, PropertyInfo property, ACItemBindingAttribute attribute, string uncastedValue)
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
                var e = from ReflectionEnumCacheItem p in _cache.EnumCache.Enumerations
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
        private Match GetMatchForCommandInterpretType(string literalCommandPart, ACItemBindingAttribute attribute, PropertyInfo property)
        {
            switch (attribute.Type)
            {
                case CommandInterpretType.SingleArgument:
                    return new Regex("\\s(?<value>" + literalCommandPart + ")\\s",
                              RegexOptions.IgnoreCase).Match(" " + literalCommandPart + " ");
                case CommandInterpretType.Flag:
                    var en = from ReflectionEnumCacheItem p in _cache.EnumCache.Enumerations where p.EnumerationType == property.PropertyType select p;
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
        public IEnumerable<IActionTrigger> Interpret(string action)
        {
            if (!IsRunning)
                throw new Exception(string.Format("Service {0} can't interpret action string. The service is not running.", DisplayName));
            try
            {
                var actionTriggers = ReflectionHelpers.Interpret(_cache.TriggerInterpreters, action, ';');
                foreach (var actionTrigger in actionTriggers)
                {
                    actionTrigger.Commands = ReflectionHelpers.Interpret(_cache.CommandInterpreters,
                                                                         actionTrigger.LiteralPart, ',');
                    foreach (var command in actionTrigger.Commands)
                    {
                        // Bind to properties which cary the ACItemBinding Attribute.
                        // todo: put the binding properties in a reflection cache from improved speed.
                        foreach (var property in command.GetType().GetProperties())
                        {
                            var o = property.GetCustomAttributes(typeof (ACItemBindingAttribute), false);
                            if (o != null && o.Length == 1)
                            {
                                var attribute = (ACItemBindingAttribute) o[0];
                                Match match = null;
                                try
                                {
                                    match = GetMatchForCommandInterpretType(command.LiteralPart, attribute, property);
                                }
                                catch
                                {

                                }
                                if (match != null && match.Success)
                                {
                                    CastValueToProperty(command, property, attribute, match.Groups["value"].Value);
                                }
                            }
                            //foreach (var attribute in property.Attributes) 
                        }
                    }
                }
                return actionTriggers;
            }
            catch
            {
                return null; // TODO: implement partial interpretation.
            }
        }

        #region IService Members

        public bool Stop()
        {
            if (!IsRunning)
                throw new Exception(string.Format("The {0} can't stop, it is not running.", DisplayName));
            _cache = null;
            IsRunning = false;
            return true;
        }

        public bool Start()
        {
            if (IsRunning)
                throw new Exception(string.Format("The {0} can't start, is already started.",DisplayName));
            _cache = new ReflectionCache();
            IsRunning = true;
            return true;
        }

        public bool IsRunning
        {
            get; private set;
        }

        #endregion

        #region IIdentifiable Members

        public string DisplayName
        {
            get { return "Action interpreter service"; }
        }

        public System.Guid Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string TechnicalName
        {
            get; set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
