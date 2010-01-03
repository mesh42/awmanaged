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
using System.Linq;
using AwManaged.Core.Interfaces;
using AwManaged.Security.RemoteBotEngine.Interfaces;
using AwManaged.Storage;
using AwManaged.Storage.Interfaces;
using Db4objects.Db4o;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Linq;

namespace AwManaged.Security.RemoteBotEngine
{
    /// <summary>
    /// Stores authorizations for objects which "implement" the IHaveAutorization interface.
    /// these could be users, groups, or anything else which needs to be autorized against a role.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class Authorization<TObject> : IAuthorization<TObject> where TObject : IHaveAuthorization
    {
        private readonly IObjectContainer _db;

        [Indexed]
        private string _role;
        private TObject _authorizable; // here's where the OOP DB storage magic lies :)

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role
        {
            get { return _role; }
        }

        /// <summary>
        /// Gets or sets the authorizable.
        /// </summary>
        /// <value>The authorizable.</value>
        public TObject Authorizable
        {
            get { return _authorizable; }
        }
        /// <summary>
        /// Adds the authorization.
        /// </summary>
        public void AddAuthorization()
        {
            // adds an authorization for the current object.
            var record = from AuthorizableWrapper p in _db where (p.Role == _role && p.Authorizable.Id == _authorizable.Id) select p;
            if (record.Count() == 1)
                throw new Exception(string.Format("can't add {0} to authorization role {1} as it already exists.", typeof(TObject), _role));
            _db.Store(new AuthorizableWrapper(){Authorizable = _authorizable, Role =_role});
            _db.Commit();
        }

        public static List<string> GetAuthorizations<TObject,TConnectionInterface>(IStorageClient<TConnectionInterface> storage, TObject authorizable)
            where TObject : IHaveAuthorization
            where TConnectionInterface : IConnection<TConnectionInterface>
        {
            var roles = new List<string>();
            var records = from AuthorizableWrapper p in storage.Db where (p.Authorizable.Id == authorizable.Id) select p;
            foreach (var item in records)
            {
                roles.Add(item.Role);
            }
            return roles;
        }

        public bool HasAuthorization()
        {
            var record = from AuthorizableWrapper p in _db where (p.Role == _role && p.Authorizable.Id == _authorizable.Id) select p;
            return (record.Count() == 1);
        }

        /// <summary>
        /// Db4o is not able to query object templates. therefore we introduce a wrapper
        /// </summary>
        private class AuthorizableWrapper
        {
            private IHaveAuthorization _authorizable;
            private string _role;

            public IHaveAuthorization Authorizable
            {
                get; set;
            }

            public string Role
            {
                get; set;
            }
        }

        /// <summary>
        /// Removes the authorization.
        /// </summary>
        public void RemoveAuthorization()
        {
            var record = from AuthorizableWrapper p in _db where (p.Role == _role && p.Authorizable.Id == _authorizable.Id) select p;
            if (record.Count() == 0)
                throw new Exception(string.Format("can't remove {0} from authorization role {1} as it does not exist.",typeof (TObject), _role));
            _db.Delete(record.Single());
            _db.Commit();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Authorization&lt;TObject&gt;"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="authorizable">The authorizable.</param>
        /// <param name="role">The role.</param>
        public Authorization(IObjectContainer db, TObject authorizable, string role)
        {
            _db = db;
            _role = role;
            _authorizable = authorizable;
        }
    }
}
