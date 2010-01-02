using AwManaged.Security.RemoteBotEngine.Interfaces;

namespace AwManaged.Security.RemoteBotEngine
{
    public class Group : IHaveAuthorization
    {
        string AwManaged.Interfaces.IIdentifiable.DisplayName
        {
            get { throw new System.NotImplementedException(); }
        }

        System.Guid AwManaged.Interfaces.IIdentifiable.Id
        {
            get { throw new System.NotImplementedException(); }
        }


    }
}
