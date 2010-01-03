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
using AwManaged.Core.Interfaces;
using AwManaged.Security.RemoteBotEngine.Interfaces;
using AwManaged.Storage.Interfaces;

namespace AwManaged.Security.RemoteBotEngine
{
    public class IdmClient<TConnectionInterface> : IIdentityManagementClient<TConnectionInterface>
        where TConnectionInterface : IConnection<TConnectionInterface>
    {

        public IdmClient(IStorageClient<TConnectionInterface> storage)
        {
            Storage = storage;
        }

        #region IIdentityManagementClient<TConnectionInterface> Members

        public AwManaged.Storage.Interfaces.IStorageClient<TConnectionInterface> Storage
        {
            get; private set;
        }

        public IUser User
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAuthorization<User> UserAuthorization
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAuthorization<Group> GroupAuthorization
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        public string DisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public string TechnicalName
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning
        {
            get { throw new NotImplementedException(); }
        }

        #region IIdentityManagementObjects Members


        public Db4objects.Db4o.IObjectContainer Db
        {
            get { return Storage.Db; }
        }

        #endregion
    }
}
