using AwManaged.Security.RemoteBotEngine.Interfaces;

namespace AwManaged.Security.RemoteBotEngine
{
    public class IdmClient<TConnectionInterface> : IIdentityManagementClient<TConnectionInterface>
    {

        #region IIdentityManagementClient<TConnectionInterface> Members

        public AwManaged.Storage.Interfaces.IStorageClient<TConnectionInterface> Client
        {
            get { throw new System.NotImplementedException(); }
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
    }
}
