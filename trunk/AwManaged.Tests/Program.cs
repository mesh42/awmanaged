using System;
using System.Collections.Generic;
using AwManaged.SceneNodes;
using AWManaged.Security;
using AwManaged.Tests;

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


            foreach (var model in bot.Model)
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