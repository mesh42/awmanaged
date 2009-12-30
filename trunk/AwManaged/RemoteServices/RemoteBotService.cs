using AwManaged.WcfServices.Interfaces;

namespace AwManaged.WcfServices
{
    public sealed class RemoteBotService : IRemoteBotService
    {
        #region ISceneNodeCommands<Model,Avatar,HudBase<Avatar>> Members

        public void HudDisplay(AwManaged.Scene.HudBase<AwManaged.Scene.Avatar> hud, AwManaged.Scene.Avatar avatar)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteObject(AwManaged.Scene.Model o)
        {
            throw new System.NotImplementedException();
        }

        public void AddObject(AwManaged.Scene.Model o)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeObject(AwManaged.Scene.Model model)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeObject(AwManaged.Scene.Model model, int delay)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
