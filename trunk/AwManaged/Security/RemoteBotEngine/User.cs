using System;
using System.Linq;
using AwManaged.Security.RemoteBotEngine.Interfaces;
using AwManaged.Storage.Interfaces;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Linq;

namespace AwManaged.Security.RemoteBotEngine
{
    public class User : IUser
    {
        [Indexed] 
        private Guid _id;
        [Indexed]
        private string _name;
        [Indexed]
        private string _nameToLower;
        [Indexed]
        private string _password;
        [Indexed]
        private string _lastLogonDate;
        [Indexed]
        private string _email;
        [Indexed]
        private string _emailToLower;
        [Indexed]
        private bool _isLockedOut;

        private int _maxConnections;
        

        public string Name
        {
            get { return _name; }
        }

        public string Password
        {
            get { return _password; }
        }

        public string LastLogonDate
        {
            get { return _lastLogonDate; }
        }

        public string Email
        {
            get { return _email; }
        }

        public string NameToLower
        {
            get { return _nameToLower; }
        }

        public string EmailToLower
        {
            get { return _emailToLower; }
        }

        public bool IsLockedOut
        {
            get { return _isLockedOut; }
        }

        public int MaxConnections
        {
            get { return _maxConnections; }
        }

        /// <summary>
        /// Registers the user using the specified storage client.
        /// </summary>
        /// <returns></returns>
        public RegistrationResult RegisterUser<TConnectionInterface>(IStorageClient<TConnectionInterface> storage)
        {
            var records = from User p in storage.Db where (p.NameToLower == NameToLower || p.EmailToLower == EmailToLower) select p;
            if (records.Count() == 1)
            {
                var u = records.Single();
                if (u.NameToLower == NameToLower)
                    return RegistrationResult.UserExists;
                if (u.EmailToLower == EmailToLower)
                    return RegistrationResult.EmailExists;
            }
            _id = Guid.NewGuid();
            storage.Db.Store(this);
            storage.Db.Commit();
            return RegistrationResult.Success;
        }

        public User(string name, string password, string email)
        {
            _name = name;
            _password = password;
            _email = email;

            _nameToLower = name.ToLower();
            _emailToLower = email.ToLower();
        }

        public User(string name, string password)
        {
            _name = name;
            _password = password;
            _nameToLower = name.ToLower();
        }

        public bool IsAuthenticated<TConnectionInterface>(IStorageClient<TConnectionInterface> storage)
        {
            var records = from User p in storage.Db where (p.Password == _password && p.NameToLower == _nameToLower) select p;
            return records.Count() == 1;
        }

        static public bool LockoutUser<TConnectionInterface>(IStorageClient<TConnectionInterface> storage, string name)
        {
            var records = from User p in storage.Db where (p.NameToLower == name.ToLower()) select p;
            if (records.Count() == 1)
            {
                var user = records.Single();
                user._isLockedOut = true;
                storage.Db.Store(user);
                storage.Db.Commit();
                return true;
            }
            return false;
        }



        #region IIdentifiable Members

        string AwManaged.Interfaces.IIdentifiable.DisplayName
        {
            get { return Name; }
        }

        System.Guid AwManaged.Interfaces.IIdentifiable.Id
        {
            get { return _id; }
        }

        #endregion
    }
}
