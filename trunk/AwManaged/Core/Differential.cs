﻿using System;
using System.Collections.Generic;
using System.Reflection;
namespace AwManaged.Core
{
    public static class Differential
    {
        public static List<PropertyInfo> Properties<T>(T x, T y)
        {
            var ret = new List<PropertyInfo>();
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var valx = property.GetValue(x, null) as IComparable;
                if (valx == null)
                    continue;
                object valy = property.GetValue(y, null);
                var compareValue = valx.CompareTo(valy);
                if (compareValue != 0)
                    ret.Add(property);
            }
            return ret;
        }
    }
}