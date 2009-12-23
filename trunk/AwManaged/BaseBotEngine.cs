using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Converters;
using AwManaged.Core;
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

        private Instance aw { get; set; }
        private Timer _timer;

        private ProtectedList<Model> _model = new ProtectedList<Model>();
        private ProtectedList<Camera> _cameras = new ProtectedList<Camera>();
        private ProtectedList<Mover> _movers = new ProtectedList<Mover>();
        private ProtectedList<ZoneObject> _zones = new ProtectedList<ZoneObject>();
        private ProtectedList<IHudBase> _huds = new ProtectedList<IHudBase>();
        private ProtectedList<Avatar> _avatar = new ProtectedList<Avatar>();

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

        #region ScanObjects Non Global 

        readonly int[,] _sequence = new int[3, 3];
        int _queryX;
        int _queryZ;
        private bool _isEnterGlobal;

        /// <summary>
        /// Scans the objects non global. incase the bot is not running under Care Taker authorization.
        /// </summary>
        private void ScanObjectsNonGlobal()
        {
            aw.EventCellBegin += new Instance.Event(handle_cell_begin);
            aw.EventCellObject += aw_EventCellObject;

            _queryX = Utility.SectorFromCell(0);
            _queryZ = Utility.SectorFromCell(0);

            do
            {
                var rc = aw.Query(_queryX, _queryZ, _sequence);
                if (rc != 0)
                    throw new AwException(rc);
            } while ((!aw.GetBool(Attributes.QueryComplete)));

            WriteLine("Scan completed, found " + _model.Count + " objects in " + LoginConfiguration.World + ".");

            _timer = new Timer(refresh, null, 0, 10);
            if (ObjectEventScanCompleted != null)
                ObjectEventScanCompleted(this, new EventObjectScanEventArgs(Model));

        }

        private void handle_cell_begin(Instance sender)
        {
            var cell_x = sender.GetInt(Attributes.CellX);
            var cell_z = sender.GetInt(Attributes.CellZ);
            var sector_x = Utility.SectorFromCell(cell_x) - _queryX;
            var sector_z = Utility.SectorFromCell(cell_z) - _queryZ;
  
            if (sector_x < -1 || sector_x > 1 || sector_z < -1 || sector_z > 1)
                return;
            _sequence[sector_z + 1,sector_x + 1] = sender.GetInt(Attributes.CellSequence);
        }


        #endregion

        #region ScanObjects Global

        public virtual void ScanObjects()
        {
            _model = new ProtectedList<Model>();

            WriteLine("Scanning objects in world" + LoginConfiguration.World + ".");

            // TODO: Dirty override, please implement this properly.
            if (!IsEnterGlobal)
            {
                ScanObjectsNonGlobal();
                return;
            }
            aw.EventCellObject += new Instance.Event(this.aw_EventCellObject);

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

        #endregion

        public void DeleteObject(Model o)
        {
            try
            {
                aw.SetInt(Attributes.ObjectId, o.Id);
                aw.SetInt(Attributes.ObjectNumber, o.Number);
                aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
                aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);
                aw.ObjectDelete();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        public void DeleteObject(int id)
        {
            try
            {
                aw.SetInt(Attributes.ObjectId, id);
                aw.ObjectDelete();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        public void AddObject(Model o)
        {
            AddObject(o.Description, o.ModelName, o.Position, o.Rotation, o.Action);
        }

        /// <summary>
        /// Adds a new object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="model">The model.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="action">The action.</param>
        public void AddObject(string description, string model, Vector3 position, Vector3 rotation, string action)
        {
            try
            {
                aw.SetString(Attributes.ObjectDescription, description);
                aw.SetString(Attributes.ObjectModel, model);
                aw.SetInt(Attributes.ObjectRoll, (int) rotation.z);
                aw.SetInt(Attributes.ObjectTilt, (int) rotation.x);
                aw.SetInt(Attributes.ObjectYaw, (int) rotation.y);
                aw.SetString(Attributes.ObjectAction, action);
                aw.SetInt(Attributes.ObjectX, (int) position.x);
                aw.SetInt(Attributes.ObjectY, (int) position.y);
                aw.SetInt(Attributes.ObjectZ, (int) position.z);
                aw.ObjectAdd();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
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

                var l = new List<string>();
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
        /// <param name="argumentType">Type of the argument.</param>
        /// <param name="avatar">The avatar.</param>
        /// <param name="message">The message.</param>
        public void Say(int delay, SessionArgumentType argumentType, IAvatar avatar,string message)
        {
            var o = new SayCallbackStruct {Session = avatar.Session, Delay = delay, Message = message};
            o.Timer = new Timer(SayCallback, o, delay, 0);
        }

        private struct SayCallbackStruct
        {
            public int Session;
            public string Message;
            public Timer Timer;
            public int Delay;
            public SessionArgumentType ArgumentType;
        }

        private void SayCallback(object state)
        {
            var result = (SayCallbackStruct) state;
            // only send a message if the session is stil available.
            if (_avatar.Exists(p => p.Session == result.Session))
            {
                Say(result.Message);
            }
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
                rc = aw.Login();
                if (rc != 0)
                    throw new AwException(rc);

                if (BotEventLoggedIn!=null)
                    BotEventLoggedIn(this,LoginConfiguration);

                //aw.EventCellObject += new Instance.Event(this.aw_EventCellObject); <-- moved to scan implementation method.
                //Have the bot enter the specified world
                aw.EventAvatarAdd += new Instance.Event(aw_EventAvatarAdd);
                aw.EventAvatarChange += new Instance.Event(aw_EventAvatarChange);
                aw.EventChat += new Instance.Event(aw_EventChat);
                aw.EventObjectBump += new Instance.Event(aw_EventObjectBump);
                aw.EventAvatarDelete += new Instance.Event(aw_EventAvatarDelete);
                aw.EventObjectClick += new Instance.Event(aw_EventObjectClick);
                aw.EventObjectDelete += new Instance.Event(aw_EventObjectDelete);
                aw.EventObjectAdd += new Instance.Event(aw_EventObjectAdd);

                if (aw.GetBool(Attributes.WorldCaretakerCapability))
                {
                    aw.SetBool(Attributes.EnterGlobal, true);
                    rc = aw.Enter(loginConfiguration.World);
                    _isEnterGlobal = true;
                    if (rc != 0)
                        throw new AwException(rc);
                }
                else
                {
                    aw.SetBool(Attributes.EnterGlobal, false);
                    _isEnterGlobal = false;
                    rc = aw.Enter(loginConfiguration.World);
                    if (rc != 0)
                    {
                        // still failed. throw  managed detailed exception.
                        throw new AwException(rc);
                    }
                }

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
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
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
            try
            {
                AddObject(sender);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        void aw_EventObjectDelete(Instance sender)
        {
            try
            {
                var o = from p in Model where p.Id == aw.GetInt(Attributes.ObjectId) select p;
                if (o.Count() != 0)
                {
                    Model.InternalRemove(o.ElementAt(0));
                }
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        void aw_EventObjectClick(Instance sender)
        {
            try
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
                        ObjectEventClick(this, new EventObjectClickArgs(o.ElementAt(0), avatar.ElementAt(0)));
                }
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        //void timerCallback(object state)
        //{
        //    try
        //    {
        //        Utility.Wait(1);
        //    }
        //    catch (InstanceException ex)
        //    {
        //        HandleExceptionManaged(ex);
        //    }
        //}

        void HandleExceptionManaged(InstanceException ex)
        {
            throw new AwException(ex.ErrorCode);
            //TODO: handle universe server diconnected exception for example. keep the bot alive.
        }

        void aw_EventAvatarDelete(Instance sender)
        {
            try
            {
                var avatar = GetAvatar(aw.GetInt(Attributes.AvatarSession));
                // update the authorization matrix.
                loginConfiguration.Authorization.Matrix.RemoveAll(
                    p => p.Role == RoleType.student && p.Citizen == aw.GetInt(Attributes.AvatarCitizen));
                _avatar.InternalRemoveAll(p => p.Session == aw.GetInt(Attributes.AvatarSession));

                if (AvatarEventRemove != null)
                    AvatarEventRemove(this, new EventAvatarRemoveArgs(avatar));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
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
            try
            {

                if (ChatEvent != null)
                {
                    var avatar = from p in _avatar where (p.Session == aw.GetInt(Attributes.ChatSession)) select p;
                    int type = aw.GetInt(Attributes.ChatType);
                    string text = aw.GetString(Attributes.ChatMessage);
                    var ct = (ChatType) type;
                    ChatEvent(this, new EventChatArgs(avatar.ElementAt(0), ct, text));

                }
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #endregion

        Model AddObject(Instance sender)
        {
            try
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
                                  (ObjectType) sender.GetInt(Attributes.ObjectType),
                                  sender.GetString(Attributes.ObjectModel),
                                  position, rotation, sender.GetString(Attributes.ObjectDescription),
                                  sender.GetString(Attributes.ObjectAction), sender.GetInt(Attributes.ObjectNumber),
                                  sender.GetString(Attributes.ObjectData));

                _model.InternalAdd(o);
                if (ObjectEventAdd != null)
                    ObjectEventAdd(this, new EventObjectAddArgs(o, GetAvatar(sender.GetInt(Attributes.AvatarSession))));
                return o;
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
            return null;
        }

        void AddCameraObject(Instance sender)
        {
            try
            {
                Camera ret;
                sender.GetV4Object(out ret);
                Cameras.InternalAdd(ret);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        void AddMoverObject(Instance sender)
        {
            try
            {
                Mover ret;
                sender.GetV4Object(out ret);
                Movers.InternalAdd(ret);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        void AddZoneObject(Instance sender)
        {
            try
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
                                  (ObjectType) aw.GetInt(Attributes.ObjectType), aw.GetString(Attributes.ObjectModel),
                                  position, rotation, aw.GetString(Attributes.ObjectDescription),
                                  aw.GetString(Attributes.ObjectAction), aw.GetInt(Attributes.ObjectNumber),
                                  aw.GetString(Attributes.ObjectData));

                sender.GetV4Object(out ret);

                var zone = new ZoneObject(ret, o);

                Zones.InternalAdd(zone);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
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

            try
            {
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
                                    (ObjectType) aw.GetInt(Attributes.ObjectType), aw.GetString(Attributes.ObjectModel),
                                    position, rotation, aw.GetString(Attributes.ObjectDescription),
                                    aw.GetString(Attributes.ObjectAction), aw.GetInt(Attributes.ObjectNumber),
                                    aw.GetString(Attributes.ObjectData));

                _model.InternalAdd(o);
                //if (ObjectEventAdd != null)
                //    ObjectEventAdd(this, new EventObjectAddArgs(o, this.GetAvatar(aw.GetInt(Attributes.AvatarSession))));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);

            }
        }

        void aw_EventAvatarChange(Instance sender)
        {
            var avatar = from p in _avatar where p.Session == aw.GetInt(Attributes.AvatarSession) select p;

            if (avatar.Count() == 0)
                return;

            try
            {

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
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);

            }

        }

        void aw_EventAvatarAdd(Instance sender)
        {
            try
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
                _avatar.InternalAdd(a);

                Whisper(RoleType.debugger, "Added :" + a.Name);

                if (AvatarEventAdd != null)
                    AvatarEventAdd(this, new EventAvatarAddArgs(a));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);

            }
        }


        private void refresh(object o)
        {
            try
            {
                Utility.Wait(0);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                aw.Dispose();
                _timer.Dispose();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #endregion

        #region IBaseBotEngine Members

        public ProtectedList<Model> Model
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

        public ProtectedList<Camera> Cameras
        {
            get { return _cameras; }
            set { _cameras = value; }
        }

        public ProtectedList<Mover> Movers
        {
            get { return _movers; }
            set { _movers = value; }
        }

        public ProtectedList<ZoneObject> Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }

        #endregion

        #region IBaseBotEngine Members

        public void ChangeObjectAction(Model o)
        {
            try
            {
                aw.SetInt(Attributes.ObjectOldNumber, 0);
                aw.SetInt(Attributes.ObjectId, o.Id);
                aw.SetString(Attributes.ObjectAction, o.Action);
                aw.ObjectChange();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        public void ChangeObject(Model o)
        {
            try
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

                    while (aw.ObjectChange() != 0)
                    {
                    }

                }
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
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


        public ProtectedList<IHudBase> Huds
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

        #region IAvatarCommands Members

        /// <summary>
        /// Gets the avatar.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public IAvatar GetAvatar(int session)
        {
            var avatar = from p in _avatar where (p.Session == session) select p;
            if (avatar.Count() == 1)
                return avatar.ElementAt(0);
            return null;
        }

        /// <summary>
        /// Sets the avatar's position.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="position">The position.</param>
        /// <param name="yaw">The yaw.</param>
        public void SetPosition(IAvatar avatar, Vector3 position, float yaw)
        {
            try
            {
                avatar.Position = position;
                aw.SetInt(Attributes.TeleportX, (int) position.x);
                aw.SetInt(Attributes.TeleportY, (int) position.y);
                aw.SetInt(Attributes.TeleportZ, (int) position.z);
                aw.SetInt(Attributes.TeleportYaw, (int)yaw);
                aw.Teleport(avatar.Session);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        /// <summary>
        /// Sets the avatar's position.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public void SetPosition(IAvatar avatar, float x, float y, float z, float yaw)
        {
            SetPosition(avatar, new Vector3(x, y, z),yaw);
        }

        #endregion

        #region IBaseBotEngine Members

        public bool IsEnterGlobal { get { return _isEnterGlobal; } }

        #endregion
    }

    public interface IEventObjectScanEventArgs
    {
        ProtectedList<Model> Model { get; set; }
    }
}