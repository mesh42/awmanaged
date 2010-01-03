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
using AwManaged.Core.Interfaces;

namespace AwManaged.Storage
{
    public sealed class StorageConfiguration<TConnectionInterface> : IConnection<TConnectionInterface>
    {

        public string ConnectionString
        {
            get; private set;
        }

        public StorageConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region IStorageConfiguration<TConnectionInterface> Members


        public TConnectionInterface Connection
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region IStorageProviderIdentity Members

        public string ProviderName
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
