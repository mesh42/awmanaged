using System.ServiceModel;
using AwManaged.Scene;
using AwManaged.Scene.Interfaces;

namespace AwManaged.WcfServices.Interfaces
{
    [ServiceContract]
    public interface IRemoteBotService : ISceneNodeCommands<Model, Avatar, HudBase<Avatar>>
    {
    }
}