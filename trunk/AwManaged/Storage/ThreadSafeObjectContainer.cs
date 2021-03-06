﻿/* **********************************************************************************
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
using System.Collections.Generic;
using AwManaged.Core.Interfaces;
using Db4objects.Db4o;

namespace AwManaged.Storage
{
    /// <summary>
    /// Creates a new db connection to the server for each thread-id.
    /// This results in a minimal connection pool, while retaining multi threaded performance.
    /// </summary>
    /// <typeparam name="TConnection">The type of the connection.</typeparam>
    public class ThreadSafeObjectContainer<TConnection> : IHaveToCleanUpMyShit
    {
        private class ObjectContainerThread
        {
            public int ThreadId { get; set; }
            public IObjectContainer Db { get; set; }
        }

        private List<ObjectContainerThread> _containers;

        public IObjectContainer GetNewInstance()
        {
            return Db4oFactory.OpenClient(Connection.HostAddress, Connection.HostPort, Connection.User, Connection.Password);
        }

        public IObjectContainer GetInstance()
        {
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (_containers.Exists(p => p.ThreadId == id))
                return _containers.Find(p => p.ThreadId == id).Db;
            else
            {
                var container = new ObjectContainerThread();
                container.ThreadId = id;
                container.Db = Db4oFactory.OpenClient(Connection.HostAddress, Connection.HostPort, Connection.User, Connection.Password);
                _containers.Add(container);
                return container.Db;
            }
        }

        public Db4OConnection Connection { get; private set;}

        public ThreadSafeObjectContainer(Db4OConnection connection)
        {
            Connection = connection;
            _containers = new List<ObjectContainerThread>();
        }

        /// <summary>
        /// Removes the instance serving the current thread id.
        /// </summary>
        public bool RemoveInstance()
        {
            bool ret = false;
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            var container = _containers.Find(p => p.ThreadId == id);
            if (container != null)
            {
                ret = container.Db.Close();
                _containers.Remove(container);
                container.Db.Dispose();
            }

            return ret;
        }

        #region IHaveToCleanUpMyShit Members

        public void Dispose()
        {
            foreach (var item in _containers)
            {
                item.Db.Close();
                item.Db.Dispose();
            }
            _containers.Clear();
            _containers = null;
        }

        #endregion
    }
}
