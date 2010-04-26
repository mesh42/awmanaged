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
using System.Reflection;

namespace AwManaged.Core.Patterns
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