using AwManaged.Core.Interfaces;
using Db4objects.Db4o;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IIdentityManagementObjects
    {
        IUser User { get; }
        IAuthorization<User> UserAuthorization { get; }
        IAuthorization<Group> GroupAuthorization { get; }
        IObjectContainer Db { get; }
    }
}
