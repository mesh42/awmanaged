using System;
using System.Collections.Generic;

namespace AwManaged.Core
{
    public static class ConnectionStringHelper
    {
        public class NameValuePair
        {
            public string Name { get; private set;}
            public string Value { get; private set;}
            public NameValuePair(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        public static void ThrowIncorrectConnectionString(string connectionString)
        {
            throw new ArgumentException(String.Format("Connectionstring is in the incorrect format '{0}'",connectionString));
        }

        /// <summary>
        /// Gets the name value pairs and checks for a match in the provider name contained within the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        public static List<NameValuePair> GetNameValuePairs(string connectionString, string providerName)
        {
            var ret = new List<NameValuePair>();
            var temp =connectionString.Split(';');
            foreach (var item in temp)
            {
                var pair = item.Split('=');
                if (pair.Length != 2)
                    ThrowIncorrectConnectionString(connectionString);
                ret.Add(new NameValuePair(pair[0].ToLower(), pair[1]));
            }

            if (ret.Find(p => p.Name == "provider" && p.Value == providerName) == null)
                ThrowIncorrectConnectionString(connectionString);

            return ret;
        }
    }
}