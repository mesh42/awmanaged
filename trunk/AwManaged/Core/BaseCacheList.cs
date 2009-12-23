using System;
using System.Collections.Generic;
using System.Security;

namespace AwManaged.Core
{
    /// <summary>
    /// Cachelist reimplements List<typeparamref name="T"/> and internalizes several methods and properties
    /// to provide a non writeable protected List.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCacheList<T> : List<T>
    {
        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Add(T item)
        {
            throw new SecurityException();
        }

        internal void InternalAdd(T item) 
        {
            base.Add(item);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void AddRange(IEnumerable<T> collection)
        {
            throw new SecurityException();
        }

        internal void InternalAddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        internal new bool Remove(T Item)
        {
            throw new SecurityException();
        }
        
        internal bool InternalRemove(T item)
        {
            return base.Remove(item);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new int RemoveAll(Predicate<T> match)
        {
            throw new SecurityException();
        }


        internal int InternalRemoveAll(Predicate<T> match)
        {
            return base.RemoveAll(match);
        }


        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void RemoveAt(int index)
        {
            throw new SecurityException();            
        }

        public void InternalRemoveAt(int index)
        {
            base.RemoveAt(index);
        }


        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void RemoveRange(int index, int count)
        {
            throw new SecurityException();            
        }

        public void InternalRemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Clear()
        {
            throw new SecurityException();
        }

        public void InternalClear()
        {
            base.Clear();
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Insert(int index, T item)
        {
            throw new SecurityException();
        }

        public void InternalInsert(int index, T item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            throw new SecurityException();
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Sort(Comparison<T> comparison)
        {
            throw new SecurityException();
        }

        public void InternalSort(Comparison<T> comparison)
        {
            base.Sort(comparison);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Sort(IComparer<T> comparer)
        {
            throw new SecurityException();
        }

        public void InternalSort(IComparer<T> comparer)
        {
            base.Sort(comparer);
        }


        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            throw new SecurityException();
        }

        public void InternalSort(int index, int count, IComparer<T> comparer)
        {
            base.Sort(index, count, comparer);
        }

        /// <summary>
        /// Throws a security exception when used. Only allowed by the internal cache's implementation.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException" />
        public new void Reverse(int index, int count)
        {
            throw new SecurityException();
        }

        public void InternalReverse(int index, int count)
        {
            base.Reverse(index, count);
        }
    }
}
