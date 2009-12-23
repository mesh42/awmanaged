using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AwManaged.Core
{
    /// <summary>
    /// Provides a list with minimal functionality exposed to prevent abuse of the AWManaged cache's.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProtectedList<T> : BaseCacheList<T>
    {
    }
}
