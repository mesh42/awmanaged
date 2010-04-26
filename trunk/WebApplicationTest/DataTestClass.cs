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
using System.ComponentModel;
using System.Linq;
using AwManaged.Core.Interfaces;
using AwManaged.LocalServices.WebServer.Attributes;
using Db4objects.Db4o;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Linq;

namespace WebApplicationTest
{
    [Description("User")]
    public class DataTestClass  : IPersist
    {
        [Indexed] private readonly DateTime _created;
        [Indexed] private Guid _id;
        [Indexed] private string _userName;
        [Indexed] private string _password;
        [Indexed] private string _firstName;
        [Indexed] private string _lastName;

        public DataTestClass()
        {
            _created = DateTime.Now;
        }

        public DataTestClass(string userName, string password, string firstName, string lastName)
        {
            _created = DateTime.Now;
            _userName = userName;
            _password = password;
            _firstName = firstName;
            _lastName = lastName;
        }

        [WebFormField("[a-z,A-Z,0-9]{1,24}", true)]
        [Description("User Id")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        [WebFormField("[a-z,A-Z]{1,24}",true)]
        [Description("First name")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [WebFormField("[a-z,A-Z]{1,24}",true)]
        [Description("Last name")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        [WebFormField("[a-z,A-Z]{1,24}",true)]

        [Description("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #region IPersist Members

        public void Persist(IObjectContainer db)
        {
            if (Id == Guid.Empty)
            {
                _id = Guid.NewGuid();
                db.Store(this);
            }
            else // user change.
            {
                var q = from DataTestClass p in db where Id == p.Id select p;
                if (q.Count() == 1)
                    db.Delete(q.Single());
            }
            db.Store(this);
        }

        #endregion
    }
}