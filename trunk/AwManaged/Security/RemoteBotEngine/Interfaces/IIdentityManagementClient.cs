using AwManaged.Storage.Interfaces;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IIdentityManagementClient<TConnectionInterface>
    {
        IStorageClient<TConnectionInterface> Client { get; }
        IUser User { get; }
        IAuthorization<User> UserAuthorization { get; }
        IAuthorization<Group> GroupAuthorization { get; }
    }
}
