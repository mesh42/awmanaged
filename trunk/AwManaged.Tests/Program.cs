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
using System.Collections.Generic;
using AwManaged.Core;
using AwManaged.Scene;
using AWManaged.Security;

namespace AwManaged.Tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            var  authorization = new Authorization();
            authorization.Matrix.Add(new CitizenRole(RoleType.debugger,0));

            // TODO: update this with your own privileges.
            var bot = new BotEngineExample();
            var buildStats = new List<BuildStat>();


            var sn1 = bot.SceneNodes;
            var sn2 = bot.SceneNodes;
            
            var model1 = sn1.Models[0];
            var model2 = sn2.Models[0];

            model1.Action = "sdkfjdlfjsdklfdjldkfjs";
            model2.Action = " dslkfjfkasdfjaklsdfjlksfj";
            
            var diff = Differential.Properties(model1,model2);

            foreach (var model in bot.SceneNodes.Models)
            {
                var buildStat = buildStats.Find(p => p.Avatar.Citizen == model.Owner);
                if (buildStat == null)
                {
                    buildStat = new BuildStat(new Avatar() {Citizen = model.Owner}, 0);
                    buildStats.Add(buildStat);
                }
                buildStat.ObjectCount++;
            }
            buildStats.Sort((p1, p2) => p2.ObjectCount.CompareTo(p1.ObjectCount));
            Console.ReadLine();
        }

        private class BuildStat
        {
            public Avatar Avatar { get; set; }
            public int ObjectCount { get; set; }

            public BuildStat(Avatar avatar, int objectCount)
            {
                Avatar = avatar;
                ObjectCount = objectCount;
            }
        }
    }
}