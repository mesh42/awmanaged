using System;
using AwManaged.Math;
using AWManaged.Security;
using AwManaged.Tests;

namespace AwManaged.Tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            var  authorization = new Authorization();
            authorization.Matrix.Add(new CitizenRole(RoleType.debugger,32366));
            var bot = new BotEngineExample(authorization, "###########", 0, 0, "##########", "testbot",
                                           "kenteq", Vector3.Zero, Vector3.Zero);

            Console.ReadLine();
        }
    }
}