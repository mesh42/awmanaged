using AwManaged.Interfaces;

namespace AwManaged.Security.RemoteBotEngine.Interfaces
{
    public interface IHaveAuthorization : IIdentifiable
    {
        // nothing here, this is just to indicate if an object can have an authorization property, it's used as a where
        // constaint in the Authorization class.
    }
}