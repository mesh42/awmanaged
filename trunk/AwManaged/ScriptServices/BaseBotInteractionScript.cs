using AwManaged.Core;
using AwManaged.Interfaces;

namespace AwManaged.ScriptServices
{
    /// <summary>
    /// Base implementation from which all bot scripts should inherit.
    /// </summary>
    public abstract class BaseBotInteractionScript : IBotInteractionScript
    {
        internal readonly IBaseBotEngine _engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBotInteractionScript"/> class.
        /// TODO: this should be called somehow when the script is started.
        /// </summary>
        /// <param name="engine">The engine.</param>
        internal BaseBotInteractionScript(IBaseBotEngine engine)
        {
            _engine = engine;
        }

        protected BaseBotInteractionScript()
        {
        }

        #region IBotEvents Members

        public event BaseBotEngine.BotEventEntersWorldDelegate BotEventEntersWorld;

        public event BaseBotEngine.BotEventLoggedInDelegate BotEventLoggedIn;

        public event BaseBotEngine.AvatarEventAddDelegate AvatarEventAdd;

        public event BaseBotEngine.AvatarEventChangeDelegate AvatarEventChange;

        public event BaseBotEngine.AvatarEventRemoveDelegate AvatarEventRemove;

        public event BaseBotEngine.ObjectEventClickDelegate ObjectEventClick;

        public event BaseBotEngine.ObjectEventAddDelegate ObjectEventAdd;

        public event BaseBotEngine.ObjectEventRemoveDelegate ObjectEventRemove;

        public event BaseBotEngine.ObjectEventScanCompletedDelegate ObjectEventScanCompleted;

        public event BaseBotEngine.ChatEventDelegate ChatEvent;

        #endregion

        #region ISceneNodes Members

        public ProtectedList<AwManaged.SceneNodes.Model> Model
        {
            get
            {
                return _engine.Model;
            }
        }

        public ProtectedList<AW.Camera> Cameras
        {
            get
            {
                return _engine.Cameras;
            }
        }

        public ProtectedList<AW.Mover> Movers
        {
            get
            {
                return _engine.Movers;
            }
        }

        public ProtectedList<AwManaged.SceneNodes.ZoneObject> Zones
        {
            get
            {
                return _engine.Zones;
            }
        }

        public ProtectedList<AwManaged.Huds.Interfaces.IHudBase> Huds
        {
            get
            {
                return _engine.Huds;
            }
        }

        #endregion
    }
}
