using System;
using AwManaged.Storage;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Linq;

namespace AwManaged.Security.RemoteBotEngine
{
    public class AuthorizationRole
    {
        [Indexed]
        private string _role;
        [Indexed]
        private User _issuedBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRole"/> class.
        /// </summary>
        /// <param name="issuedBy">The issued by.</param>
        /// <param name="role">The role.</param>
        public AuthorizationRole(User issuedBy, string role)
        {
            _issuedBy = issuedBy;
            _role = role;
        }

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public User IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = value; }
        }

        public void RemoveRole(Db4OStorageClient storage)
        {
            // adds an authorization for the current object.
            var record = from AuthorizationRole p in storage.Db where (p.Role == _role) select p;
            if (record.Count() == 0)
                throw new Exception(string.Format("can't role {0} as it does not exist.", _role));
            storage.Db.Store(this);
            storage.Db.Commit();

        }
    }
}
