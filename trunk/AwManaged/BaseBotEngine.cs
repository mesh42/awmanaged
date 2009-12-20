using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Converters;
using AwManaged.EventHandling;
using AwManaged.EventHandling.Interfaces;
using AwManaged.ExceptionHandling;
using AwManaged.Huds.Interfaces;
using AwManaged.Interfaces;
using AwManaged.Logging;
using AwManaged.Math;
using AwManaged.SceneNodes;
using AwManaged.SceneNodes.Interfaces;
using AWManaged.Security;
using Bot.Logic.AWManaged;

namespace AwManaged
{
    public abstract class BaseBotEngine : IBaseBotEngine
    {
        #region Fields

        private LoginConfiguration loginConfiguration;
        public IBaseBotEngine BotEngine { get; set; }

        internal Instance aw { get; set; }
        private Timer _timer;

        private List<Model> _model = new List<Model>();
        private List<Camera> _cameras = new List<Camera>();
        private List<Mover> _movers = new List<Mover>();
        private List<ZoneObject> _zones = new List<ZoneObject>();
        private List<IHudBase> _huds = new List<IHudBase>();
        private List<Avatar> _avatar = new List<Avatar>();

        #endregion

        #region Properties

        
        #endregion

        #region Delegates and Events

        public delegate void BotEventLoggedInDelegate(IBaseBotEngine sender, ILoginConfiguration e);
        public event BotEventLoggedInDelegate BotEventLoggedIn;

        public delegate void BotEventEntersWorldDelegate(IBaseBotEngine sender, ILoginConfiguration e);
        public event BotEventEntersWorldDelegate BotEventEntersWorld;

        public delegate void AvatarEventAddDelegate(IBaseBotEngine sender, IEventAvatarAddArgs e);
        public event AvatarEventAddDelegate AvatarEventAdd;

        public delegate void AvatarEventChangeDelegate(IBaseBotEngine sender, IEventAvatarAddArgs e);
        public event AvatarEventChangeDelegate AvatarEventChange;

        public delegate void AvatarEventRemoveDelegate(IBaseBotEngine sender, IEventAvatarRemoveArgs e);
        public event AvatarEventRemoveDelegate AvatarEventRemove;

        public delegate void ObjectEventClickDelegate(IBaseBotEngine sender, IEventObjectClickArgs e);
        public event ObjectEventClickDelegate ObjectEventClick;

        public delegate void ObjectEventAddDelegate(IBaseBotEngine sender, IEventObjectAddArgs e);
        public event ObjectEventAddDelegate ObjectEventAdd;

        public delegate void ObjectEventRemoveDelegate(IBaseBotEngine sender, IEventObjectRemoveArgs e);
        public event ObjectEventRemoveDelegate ObjectEventRemove;

        public delegate void ObjectEventScanCompletedDelegate(IBaseBotEngine sender, IEventObjectScanEventArgs e);
        public event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;

        public delegate void ChatEventDelegate(object sender, IEventChatArgs e);
        public event ChatEventDelegate ChatEvent;

        #endregion

        #region IBaseBotEngine Implementation

        public virtual void ScanObjects()
        {
            _model = new List<Model>();

            WriteLine("Scanning objects in world" + LoginConfiguration.World + ".");

            aw.SetBool(Attributes.CellCombine, true);
            aw.SetInt(Attributes.CellIterator, 0);
            try
            {
                do
                {
                    aw.CellNext();
                } while (!aw.GetBool(Attributes.QueryComplete) && aw.GetInt(Attributes.CellIterator) != -1);

            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                Whisper(RoleType.debugger, ex.Message);
            }

            WriteLine("Scan completed, found " + _model.Count + " objects in " + LoginConfiguration.World + ".");

            _timer = new Timer(refresh, null, 0, 10);
            if (ObjectEventScanCompleted != null)
                ObjectEventScanCompleted(this, new EventObjectScanEventArgs(Model));
        }

        public void DeleteObject(Model o)
        {
            aw.SetInt(Attributes.ObjectId, o.Id);
            aw.SetInt(Attributes.ObjectNumber, o.Number);
            aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
            aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);
            aw.ObjectDelete();
        }

        public void DeleteObject(int id)
        {
            aw.SetInt(Attributes.ObjectId, id);
            aw.ObjectDelete();
        }

        public void AddObject(Model o)
        {
            AddObject(o.Description, o.ModelName, o.Position, o.Rotation, o.Action);
        }

        public void AddObject(string description, string model, Vector3 position, Vector3 rotation, string action)
        {
            aw.SetString(Attributes.ObjectDescription, description);
            aw.SetString(Attributes.ObjectModel, model);
            aw.SetInt(Attributes.ObjectRoll, (int)rotation.z);
            aw.SetInt(Attributes.ObjectTilt, (int)rotation.x);
            aw.SetInt(Attributes.ObjectYaw, (int)rotation.y);
            aw.SetString(Attributes.ObjectAction, action);
            aw.SetInt(Attributes.ObjectX, (int)position.x);
            aw.SetInt(Attributes.ObjectY, (int)position.y);
            aw.SetInt(Attributes.ObjectZ, (int)position.z);
            aw.ObjectAdd();
        }

        /// <summary>
        /// Whispers a message to all user within a specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="message">The message.</param>
        public void Whisper(RoleType role, string message)
        {
            foreach (var a in from p in _avatar select p)
            {
                if (loginConfiguration.Authorization.IsInRole(role, a.Citizen))
                {
                    aw.Whisper(a.Session, message);
                }
            }
        }

        /// <summary>
        /// Whispers a message to the current avatar session.
        /// </summary>
        /// <param name="message">The message.</param>
        [Obsolete]
        public void Whisper(string message)
        {
            aw.Whisper(aw.GetInt(Attributes.AvatarSession), message);
        }

        /// <summary>
        /// Whispers a message to the a specific avatar
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        public void Whisper(IAvatar avatar, string message)
        {
            aw.Whisper(avatar.Session, message);

        }

        public void Say(string message)
        {
            aw.Say(message);
        }

        /// <summary>
        /// Sends a message to the chat room with a specified delay.
        /// If at the time the specified avatar session is not available,
        /// the message will be not be echoed to the chat room.
        /// Great for Greeter bots to prevent greeting message flooding.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        public void Say(int delay, IAvatar avatar,string message)
        {
            var o = new SayCallbackStruct {Avatar = avatar, Delay = delay, Message = message};
            o.Timer = new Timer(SayCallback, o, delay, 0);
        }

        private struct SayCallbackStruct
        {
            public IAvatar Avatar;
            public string Message;
            public Timer Timer;
            public int Delay;
        }

        private void SayCallback(object state)
        {
            var result = (SayCallbackStruct) state;
            // only send a message if the session is stil available.
            if (_avatar.Exists(p => p.Session == result.Avatar.Session))
            {
                Say(result.Message);
            }
            result.Timer.Dispose();
        }


        public Avatar GetAvatar(int session)
        {
            var avatar = from p in _avatar where (p.Session == session) select p;
            if (avatar.Count() == 1)
                return avatar.ElementAt(0);
            return null;
        }

        public virtual void Start()
        {
            try
            {
                int rc;
                aw = new Instance(loginConfiguration.Domain, loginConfiguration.Port);
                //greeter
                //Set the login attributes and log the bot into the universe
                aw.SetString(Attributes.LoginName, loginConfiguration.LoginName);
                aw.SetString(Attributes.LoginPrivilegePassword, loginConfiguration.PrivilegePassword);
                aw.SetInt(Attributes.LoginOwner, loginConfiguration.Owner);
                aw.SetBool(Attributes.EnterGlobal, true);
                rc = aw.Login();
                if (rc != 0)
                    throw new AwException(rc);

                if (BotEventLoggedIn!=null)
                    BotEventLoggedIn(this,LoginConfiguration);

                aw.EventCellObject += new Instance.Event(this.aw_EventCellObject);
                //Have the bot enter the specified world
                aw.EventAvatarAdd += new Instance.Event(aw_EventAvatarAdd);
                aw.EventAvatarChange += new Instance.Event(aw_EventAvatarChange);
                aw.EventChat += new Instance.Event(aw_EventChat);
                aw.EventObjectBump += new Instance.Event(aw_EventObjectBump);
                aw.EventAvatarDelete += new Instance.Event(aw_EventAvatarDelete);
                aw.EventObjectClick += new Instance.Event(aw_EventObjectClick);
                aw.EventObjectDelete += new Instance.Event(aw_EventObjectDelete);
                aw.EventObjectAdd += new Instance.Event(aw_EventObjectAdd);
                rc = aw.Enter(loginConfiguration.World);
                if (rc != 0)
                    throw new AwException(rc);

                if (BotEventEntersWorld != null)
                    BotEventEntersWorld(this, LoginConfiguration);


                //Have the bot change state to 0n 0w 0a
                aw.SetInt(Attributes.MyX, (int)loginConfiguration.Position.x); //X position of the bot (E/W)
                aw.SetInt(Attributes.MyY, (int)loginConfiguration.Position.y); //Y position of the bot (height)
                aw.SetInt(Attributes.MyZ, (int)loginConfiguration.Position.z); //Z position of the bot (N/S)

                aw.SetInt(Attributes.MyPitch, (int)loginConfiguration.Position.y);
                aw.SetInt(Attributes.MyYaw, (int)loginConfiguration.Position.z);

                rc = aw.StateChange();
                if (rc != 0)
                    throw new AwException(rc);

            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                throw (ex);
            }
        }

        #endregion

        #region Constructors

        [Obsolete("Please use loginConfiguration database.")]
        protected BaseBotEngine(Authorization authorization, string domain, int port, int loginOwner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
        {
            loginConfiguration = new LoginConfiguration(authorization, domain, port, loginOwner, privilegePassword, loginName, world, position, rotation);
            aw = new Instance(loginConfiguration.Domain, loginConfiguration.Port);

        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBotEngine"/> class.
        /// Creates a new bot without a configuration
        /// </summary>
        protected BaseBotEngine()
        {
            loginConfiguration = new LoginConfiguration();
        }

        protected BaseBotEngine(string configurationName)
        {
            var configuration = new LoginConfiguration(configurationName);
        }

        protected BaseBotEngine(LoginConfiguration loginConfiguration)
        {
            this.loginConfiguration = loginConfiguration;
        }

        #endregion 

        #region Aw.Core EventHandling

        void aw_EventObjectAdd(Instance sender)
        {
            AddObject(sender);
        }

        void aw_EventObjectDelete(Instance sender)
        {
            var o = from p in Model where p.Id == aw.GetInt(Attributes.ObjectId) select p;
            if (o.Count() != 0)
            {
                Model.Remove(o.ElementAt(0));
            }
        }

        void aw_EventObjectClick(Instance sender)
        {
            var o = from p in _model where p.Id == aw.GetInt(Attributes.ObjectId) select p;
            var avatar = from p in _avatar where p.Session == aw.GetInt(Attributes.AvatarSession) select p;
            if (o.Count() == 0)
            {
                Model p = AddObject(sender);
                // object doest not exist in the local cache, this should not happen.
                if (ObjectEventClick != null)
                    ObjectEventClick(this, new EventObjectClickArgs(p, avatar.ElementAt(0)));
            }
            else
            {
                if (ObjectEventClick != null)
                    ObjectEventClick(this, new EventObjectClickArgs(o.ElementAt(0),avatar.ElementAt(0)));
            }
        }

        void timerCallback(object state)
        {
            try
            {
                Utility.Wait(1);
            }
            catch {}
        }

        void aw_EventAvatarDelete(Instance sender)
        {
            // update the authorization matrix.
            loginConfiguration.Authorization.Matrix.RemoveAll(
                p => p.Role == RoleType.student && p.Citizen == aw.GetInt(Attributes.AvatarCitizen));
            _avatar.RemoveAll(p => p.Session == aw.GetInt(Attributes.AvatarSession));

            if (AvatarEventRemove != null)
                AvatarEventRemove(this, new EventAvatarRemoveArgs(aw.GetInt(Attributes.AvatarSession)));
        }

        public void AwExceptionHandler(Exception ex)
        {
            Whisper(RoleType.debugger, ex.Message);
            Whisper(RoleType.debugger, ex.StackTrace);
        }

        void aw_EventObjectBump(Instance sender)
        {
        }

        void aw_EventChat(Instance sender)
        {

            if (ChatEvent != null)
            {
                var avatar = from p in _avatar where (p.Session == aw.GetInt(Attributes.ChatSession)) select p;
                int type = aw.GetInt(Attributes.ChatType);
                string text = aw.GetString(Attributes.ChatMessage);
                var ct = (ChatType)type;
                ChatEvent(this, new EventChatArgs(avatar.ElementAt(0), ct,text));

            }
            return;
        }

        #endregion

        Model AddObject(Instance sender)
        {
            var position = new Vector3
                               {
                                   x = sender.GetInt(Attributes.ObjectX),
                                   y = sender.GetInt(Attributes.ObjectY),
                                   z = sender.GetInt(Attributes.ObjectZ)
                               };

            var rotation = new Vector3
                               {
                                   x = sender.GetInt(Attributes.ObjectTilt),
                                   y = sender.GetInt(Attributes.ObjectYaw),
                                   z = sender.GetInt(Attributes.ObjectRoll)
                               };

            var o = new Model(sender.GetInt(Attributes.ObjectId),
                                  sender.GetInt(Attributes.ObjectOwner),
                                  AwConvert.ConvertFromUnixTimestamp(sender.GetInt(Attributes.ObjectBuildTimestamp)),
                                  (ObjectType)sender.GetInt(Attributes.ObjectType), sender.GetString(Attributes.ObjectModel),
                                  position, rotation, sender.GetString(Attributes.ObjectDescription),
                                  sender.GetString(Attributes.ObjectAction), sender.GetInt(Attributes.ObjectNumber), sender.GetString(Attributes.ObjectData));

            _model.Add(o);
            if (ObjectEventAdd != null)
                ObjectEventAdd(this, new EventObjectAddArgs(o, this.GetAvatar(sender.GetInt(Attributes.AvatarSession))));
            return o;

        }

        void AddCameraObject(Instance sender)
        {
            Camera ret;    
            sender.GetV4Object(out ret);
            Cameras.Add(ret);
        }

        void AddMoverObject(Instance sender)
        {
            Mover ret;
            sender.GetV4Object(out ret);
            Movers.Add(ret);
        }

        void AddZoneObject(Instance sender)
        {
            Zone ret;

            var position = new Vector3
                               {
                                   x = aw.GetInt(Attributes.ObjectX),
                                   y = aw.GetInt(Attributes.ObjectY),
                                   z = aw.GetInt(Attributes.ObjectZ)
                               };

            var rotation = new Vector3
                               {
                                   x = aw.GetInt(Attributes.ObjectTilt),
                                   y = aw.GetInt(Attributes.ObjectYaw),
                                   z = aw.GetInt(Attributes.ObjectRoll)
                               };


            var o = new Model(aw.GetInt(Attributes.ObjectId),
                                  aw.GetInt(Attributes.ObjectOwner),
                                  AwConvert.ConvertFromUnixTimestamp(aw.GetInt(Attributes.ObjectBuildTimestamp)),
                                  (ObjectType)aw.GetInt(Attributes.ObjectType), aw.GetString(Attributes.ObjectModel),
                                  position, rotation, aw.GetString(Attributes.ObjectDescription),
                                  aw.GetString(Attributes.ObjectAction), aw.GetInt(Attributes.ObjectNumber), aw.GetString(Attributes.ObjectData));

            sender.GetV4Object(out ret);

            var zone = new ZoneObject(ret, o);

            Zones.Add(zone);
        }

        void aw_EventCellObject(Instance sender)
        {
            switch (aw.GetInt(Attributes.ObjectType))
            {
                case (int)AW.ObjectType.Camera:
                    AddCameraObject(sender);
                    return;
                case (int)AW.ObjectType.Mover:
                    AddMoverObject(sender);
                    return;
                case (int)AW.ObjectType.Zone:
                    AddZoneObject(sender);
                    return;
            }



            //AddObject();            
            var position = new Vector3
                               {
                                   x = aw.GetInt(Attributes.ObjectX),
                                   y = aw.GetInt(Attributes.ObjectY),
                                   z = aw.GetInt(Attributes.ObjectZ)
                               };

            var rotation = new Vector3
                               {
                                   x = aw.GetInt(Attributes.ObjectTilt),
                                   y = aw.GetInt(Attributes.ObjectYaw),
                                   z = aw.GetInt(Attributes.ObjectRoll)
                               };

            Model o = new Model(aw.GetInt(Attributes.ObjectId),
                                  aw.GetInt(Attributes.ObjectOwner),
                                  AwConvert.ConvertFromUnixTimestamp(aw.GetInt(Attributes.ObjectBuildTimestamp)),
                                  (ObjectType)aw.GetInt(Attributes.ObjectType), aw.GetString(Attributes.ObjectModel),
                                  position, rotation, aw.GetString(Attributes.ObjectDescription),
                                  aw.GetString(Attributes.ObjectAction), aw.GetInt(Attributes.ObjectNumber), aw.GetString(Attributes.ObjectData));

            _model.Add(o);
            //if (ObjectEventAdd != null)
            //    ObjectEventAdd(this, new EventObjectAddArgs(o, this.GetAvatar(aw.GetInt(Attributes.AvatarSession))));
        }

        void aw_EventAvatarChange(Instance sender)
        {
            var avatar = from p in _avatar where p.Session == aw.GetInt(Attributes.AvatarSession) select p;

            if (avatar.Count() == 0)
                return;

            avatar.ElementAt(0).Position = new Vector3
                                               {
                                                   x = aw.GetInt(Attributes.AvatarX),
                                                   y = aw.GetInt(Attributes.AvatarY),
                                                   z = aw.GetInt(Attributes.AvatarZ)
                                               };

            avatar.ElementAt(0).Rotation = new Vector3
                                               {
                                                   x = aw.GetInt(Attributes.AvatarPitch),
                                                   y = aw.GetInt(Attributes.AvatarYaw),
                                                   z = 0
                                               };

            avatar.ElementAt(0).ChangedPosition();
            avatar.ElementAt(0).State = aw.GetInt(Attributes.AvatarState);
            avatar.ElementAt(0).Gesture = aw.GetInt(Attributes.AvatarGesture);
        }

        void aw_EventAvatarAdd(Instance sender)
        {
            var position = new Vector3
                               {
                                   x = aw.GetInt(Attributes.AvatarX),
                                   y = aw.GetInt(Attributes.AvatarY),
                                   z = aw.GetInt(Attributes.AvatarZ)
                               };

            var rotation = new Vector3
                               {
                                   x = aw.GetInt(Attributes.AvatarPitch),
                                   y = aw.GetInt(Attributes.AvatarYaw),
                                   z = 0
                               };


            //greeter.Say("hello");
            var a = new Avatar(
                aw.GetInt(Attributes.AvatarSession),
                aw.GetString(Attributes.AvatarName),
                position,
                rotation,
                aw.GetInt(Attributes.AvatarGesture),
                aw.GetInt(Attributes.AvatarCitizen),
                aw.GetInt(Attributes.AvatarPrivilege),
                aw.GetInt(Attributes.AvatarState)

                );
            _avatar.Add(a);

            Whisper(RoleType.debugger, "Added :" + a.Name);

            if (AvatarEventAdd != null)
                AvatarEventAdd(this, new EventAvatarAddArgs(a));
        }


        private void refresh(object o)
        {
            Utility.Wait(0);
        }

        #region IDisposable Members

        public void Dispose()
        {
            aw.Dispose();
            _timer.Dispose();

        }

        #endregion


        #region IBaseBotEngine Members

        public new List<Model> Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        public List<Camera> Cameras
        {
            get { return _cameras; }
            set { _cameras = value; }
        }

        public List<Mover> Movers
        {
            get { return _movers; }
            set { _movers = value; }
        }

        public List<ZoneObject> Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }

        #endregion

        #region IBaseBotEngine Members

        public void ChangeObjectAction(Model o)
        {
            aw.SetInt(Attributes.ObjectOldNumber, 0);
            aw.SetInt(Attributes.ObjectId, o.Id);
            aw.SetString(Attributes.ObjectAction, o.Action);
            aw.ObjectChange();
        }

        public void ChangeObject(Model o)
        {
            lock (aw)
            {
                aw.SetInt(Attributes.ObjectId, o.Id);

                aw.SetInt(Attributes.ObjectOwner, o.Owner);
                aw.SetInt(Attributes.ObjectType, (int) o.Type);
                aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
                aw.SetInt(Attributes.ObjectY, (int) o.Position.y);
                aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);

                aw.SetInt(Attributes.ObjectTilt, (int) o.Rotation.x);
                aw.SetInt(Attributes.ObjectYaw, (int) o.Rotation.y);
                aw.SetInt(Attributes.ObjectRoll, (int) o.Rotation.z);

                aw.SetString(Attributes.ObjectDescription, o.Description);
                aw.SetString(Attributes.ObjectAction, o.Action);
                aw.SetString(Attributes.ObjectModel, o.ModelName);

                if (o.Data != null)
                    aw.SetString(AW.Attributes.ObjectData, o.Data);

                while (aw.ObjectChange() != 0){}

            }
        }

        #endregion

        #region IBaseBotEngine Members


        public bool IsEchoChat
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IBaseBotEngine Members


        public LoginConfiguration LoginConfiguration
        {
            get { return loginConfiguration; }
        }

        #endregion

        #region IConfigurable Members

        public IConfiguration Configuration
        {
            get
            {
                return loginConfiguration;
            }
            set
            {
                loginConfiguration = (LoginConfiguration)value;
            }
        }

        #endregion

        #region ICanLog Members

        public ILogger logger { get; set; }

        public void WriteLine(string text)
        {
            if (logger != null)
                logger.WriteLine(text);
        }

        #endregion

        #region Initialize Members

        public void Initialize()
        {
            WriteLine("Managed AW SDK Initializing.");
        }

        #endregion

        #region IConfigurable Members

        public IList<IConfigurable> Children{ get; set; }

        #endregion

        #region IBaseBotEngine Members


        public List<IHudBase> Huds
        {
            get
            {
                return _huds;
            }
            set
            {
                _huds = value;
            }
        }

        #endregion
    }

    public interface IEventObjectScanEventArgs
    {
        List<Model> Model { get; set; }
    }
}