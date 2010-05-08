using SharedMemory;using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using AwManaged.Properties;

namespace AwManaged.Core
{
    public sealed class Resource
    {
        private static ResourceManager _manager = new  ResourceManager("Resources.resx",typeof(Resource).Assembly);


        public static string GetString(string name)
        {
            return _manager.GetString(name);
        }
    }
}
