using System;
using System.Reflection;
using AwManaged.Scene.ActionInterpreter.Interface;
using NUnit.Framework;
using System.Collections.Generic;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class ActionInterpreterTests 
    {
        private List<ITrigger> GetTriggerObjects()
        {
            Assembly triggerInterpreter = Assembly.LoadFrom("AwManaged.dll");
            List<ITrigger> actionInterpreters = new List<ITrigger>();
            foreach (var type in triggerInterpreter.GetTypes())
            {
                Type t = type.GetInterface("ITrigger");
                if (t != null)
                {
                    // create an instance of all the objects that have ITrigger.
                     var o = Activator.CreateInstance(type);
                     actionInterpreters.Add((ITrigger) o);
                }

            }
            return actionInterpreters;
        }

        [Test]
        public void ActionResolveTest()
        {
            var triggerInterpertes = GetTriggerObjects(); // this should becached upon startup to increase performance.

            var action = "activate light type=spot color=blue name=ceiling pitch=45;create sign bcolor=red; activate media url=http://212.109.3.19:8000";
            var triggers = new List<ITrigger>();

            // split up action triggers
            foreach (var trigger in action.Split(';'))
            {
                foreach (var triggerInstance in triggerInterpertes)
                {
                    if (triggerInstance.LiteralCommand.ToLower() == trigger.ToLower().Substring(0,triggerInstance.LiteralCommand.Length))
                    {
                        // we now have the trigger object reference. we should create a new object based on this type as there a re multiple types of the same trigger object
                        // in the interpreter.
                        triggers.Add((ITrigger)Activator.CreateInstance(triggerInstance.GetType()));
                    }
                }
                //var triggerType = trigger.Split(' ')[0];

                // find the object that is responsible for handling this trigger.



                //if (triggerType == string.Empty)
                //    throw new Exception("Missing action trigger type.");
                //triggers.Add(triggerType);
                //var commandSet = trigger.Replace(triggerType, string.Empty).Trim();
            }
        }
    }
}
