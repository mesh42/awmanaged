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
            authorization.Matrix.Add(new CitizenRole(RoleType.debugger,0));
            var bot = new BotEngineExample(authorization, "3dworlds.nl", 7100, 0, "", "",
                                           "", Vector3.Zero, Vector3.Zero);

            Console.ReadLine();
        }
    }
}