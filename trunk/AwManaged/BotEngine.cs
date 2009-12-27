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
using AwManaged.ExceptionHandling;
using AwManaged.Interfaces;
using AwManaged.Logging;
using AwManaged.Math;
using AwManaged.SceneNodes;
using AWManaged.Security;
using Camera=AwManaged.SceneNodes.Camera;

namespace AwManaged
{
    public abstract class BotEngine : IBotEngine<Avatar,Model,SceneNodes.Camera,SceneNodes.Zone,SceneNodes.Mover,HudBase<Avatar>>
    {
        #region Fields
        private LoginConfiguration _loginConfiguration;
        private Instance aw { get; set; }
        private Timer _timer;
        private ProtectedList<SceneNodes.Model> _model = new ProtectedList<SceneNodes.Model>();
        private SceneNodes.Model _modelRemoved;

        private ProtectedList<SceneNodes.Camera> _cameras = new ProtectedList<SceneNodes.Camera>();
        private ProtectedList<SceneNodes.Mover> _movers = new ProtectedList<SceneNodes.Mover>();
        private ProtectedList<SceneNodes.Zone> _zones = new ProtectedList<SceneNodes.Zone>();
        private ProtectedList<SceneNodes.HudBase<Avatar>> _huds = new ProtectedList<SceneNodes.HudBase<Avatar>>();
        private ProtectedList<SceneNodes.Avatar> _avatar = new ProtectedList<Avatar>();

        #endregion

        #region Properties
        public ProtectedList<SceneNodes.Model> Model { get { lock (this){return _model.Clone();} } }
        public ProtectedList<SceneNodes.Camera> Cameras { get { lock(this){ return _cameras.Clone();} } }
        public ProtectedList<SceneNodes.Mover> Movers { get { lock (this){return _movers.Clone();} } }
        public ProtectedList<SceneNodes.Zone> Zones { get { lock (this){return _zones.Clone();} } }
        public ProtectedList<SceneNodes.HudBase<Avatar>> Huds { get { lock (this){return _huds.Clone();} } }
        #endregion

        #region Delegates and Events

        public delegate void BotEventLoggedInDelegate(BotEngine sender, ILoginConfiguration e);
        public event BotEventLoggedInDelegate BotEventLoggedIn;
        public delegate void BotEventEntersWorldDelegate(BotEngine sender, ILoginConfiguration e);
        public event BotEventEntersWorldDelegate BotEventEntersWorld;
        public delegate void AvatarEventAddDelegate(BotEngine sender, EventAvatarAddArgs e);
        public event AvatarEventAddDelegate AvatarEventAdd;
        public delegate void AvatarEventChangeDelegate(BotEngine sender, EventAvatarAddArgs e);
        public event AvatarEventChangeDelegate AvatarEventChange;
        public delegate void AvatarEventRemoveDelegate(BotEngine sender, EventAvatarRemoveArgs e);
        public event AvatarEventRemoveDelegate AvatarEventRemove;
        public delegate void ObjectEventClickDelegate(BotEngine sender, EventObjectClickArgs e);
        public event ObjectEventClickDelegate ObjectEventClick;
        public delegate void ObjectEventAddDelegate(BotEngine sender, EventObjectAddArgs e);
        public event ObjectEventAddDelegate ObjectEventAdd;
        public delegate void ObjectEventRemoveDelegate(BotEngine sender, EventObjectRemoveArgs e);
        public event ObjectEventRemoveDelegate ObjectEventRemove;
        public delegate void ObjectEventScanCompletedDelegate(BotEngine sender, EventObjectScanCompletedEventArgs e);
        public event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        public delegate void ChatEventDelegate(BotEngine sender, EventChatArgs e);
        public event ChatEventDelegate ChatEvent;
        public delegate void ObjectEventChangeDelegate(BotEngine sender, EventObjectChangeArgs e);
        public event ObjectEventChangeDelegate ObjectEventChange;

        #endregion

        #region IBaseBotEngine Implementation

        #region ScanObjects Non Global

        readonly int[,] _sequence = new int[3, 3];
        int _queryX;
        int _queryZ;

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
                ObjectEventScanCompleted.Invoke(this, new EventObjectScanCompletedEventArgs(Model));

        }

        #endregion

        #region ScanObjects Global

        public virtual void ScanObjects()
        {
            try
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
                do
                {
                    aw.CellNext();
                } while (!aw.GetBool(Attributes.QueryComplete) && aw.GetInt(Attributes.CellIterator) != -1);

                WriteLine("Scan completed, found " + _model.Count + " objects in " + LoginConfiguration.World + ".");

                _timer = new Timer(refresh, null, 0, 10);
                if (ObjectEventScanCompleted != null)
                    ObjectEventScanCompleted.Invoke(this, new EventObjectScanCompletedEventArgs(Model));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #endregion

        public void DeleteObject(Model o)
        {
            lock (this)
            {
                try
                {
                    aw.SetInt(Attributes.ObjectId, o.Id);
                    //aw.SetInt(Attributes.ObjectNumber, o.Number);
                    //aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
                    // aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);
                    aw.ObjectDelete();
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
        }

        public void AddObject(Model o)
        {
            lock (this)
            {
                try
                {
                    aw.SetString(Attributes.ObjectDescription, o.Description);
                    aw.SetString(Attributes.ObjectModel, o.ModelName);
                    aw.SetInt(Attributes.ObjectRoll, (int) o.Rotation.z);
                    aw.SetInt(Attributes.ObjectTilt, (int) o.Rotation.x);
                    aw.SetInt(Attributes.ObjectYaw, (int) o.Rotation.y);
                    aw.SetString(Attributes.ObjectAction, o.Action);
                    aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
                    aw.SetInt(Attributes.ObjectY, (int) o.Position.y);
                    aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);
                    aw.ObjectAdd();
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
        }

        public virtual void Start()
        {
            try
            {
                int rc;
                aw = new Instance(_loginConfiguration.Domain, _loginConfiguration.Port);
                //greeter
                //Set the login attributes and log the bot into the universe
                aw.SetString(Attributes.LoginName, _loginConfiguration.LoginName);
                aw.SetString(Attributes.LoginPrivilegePassword, _loginConfiguration.PrivilegePassword);
                aw.SetInt(Attributes.LoginOwner, _loginConfiguration.Owner);
                rc = aw.Login();
                if (rc != 0)
                    HandleExceptionManaged(rc);

                if (BotEventLoggedIn!=null)
                    BotEventLoggedIn(this,LoginConfiguration);

                aw.EventAvatarAdd += aw_EventAvatarAdd;
                aw.EventAvatarChange += aw_EventAvatarChange;
                aw.EventChat += aw_EventChat;
                aw.EventAvatarDelete += aw_EventAvatarDelete;
                aw.EventObjectClick += aw_EventObjectClick;
                aw.EventObjectDelete += aw_EventObjectDelete;
                aw.EventObjectAdd += aw_EventObjectAdd;

                // Have the bot enter the specified world under Care Taker Privileges (default/prefered)
                if (aw.GetBool(Attributes.WorldCaretakerCapability))
                {
                    aw.SetBool(Attributes.EnterGlobal, true);
                    rc = aw.Enter(_loginConfiguration.World);
                    IsEnterGlobal = true;
                    if (rc != 0)
                        HandleExceptionManaged(rc);
                }
                else
                {
                    aw.SetBool(Attributes.EnterGlobal, false);
                    IsEnterGlobal = false;
                    rc = aw.Enter(_loginConfiguration.World);
                    if (rc != 0)
                    {
                        HandleExceptionManaged(rc);
                    }
                }

                if (BotEventEntersWorld != null)
                    BotEventEntersWorld.Invoke(this, LoginConfiguration);

                //Have the bot change state to 0n 0w 0a
                aw.SetInt(Attributes.MyX, (int)_loginConfiguration.Position.x); //X position of the bot (E/W)
                aw.SetInt(Attributes.MyY, (int)_loginConfiguration.Position.y); //Y position of the bot (height)
                aw.SetInt(Attributes.MyZ, (int)_loginConfiguration.Position.z); //Z position of the bot (N/S)

                aw.SetInt(Attributes.MyPitch, (int)_loginConfiguration.Position.y);
                aw.SetInt(Attributes.MyYaw, (int)_loginConfiguration.Position.z);

                rc = aw.StateChange();
                if (rc != 0)
                    HandleExceptionManaged(rc);

            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #endregion

        #region Constructors

        [Obsolete("Please use loginConfiguration database.")]
        protected BotEngine(Authorization authorization, string domain, int port, int loginOwner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
        {
            _loginConfiguration = new LoginConfiguration(authorization, domain, port, loginOwner, privilegePassword, loginName, world, position, rotation);
            aw = new Instance(_loginConfiguration.Domain, _loginConfiguration.Port);
        }
        
        protected BotEngine(string configurationName)
        {
            var configuration = new LoginConfiguration(configurationName);
        }

        protected BotEngine(LoginConfiguration loginConfiguration)
        {
            this._loginConfiguration = loginConfiguration;
        }

        #endregion

        #region Aw.Core EventHandling
        void handle_cell_begin(Instance sender)
        {
            lock (this)
            {
                var cell_x = sender.GetInt(Attributes.CellX);
                var cell_z = sender.GetInt(Attributes.CellZ);
                var sector_x = Utility.SectorFromCell(cell_x) - _queryX;
                var sector_z = Utility.SectorFromCell(cell_z) - _queryZ;

                if (sector_x < -1 || sector_x > 1 || sector_z < -1 || sector_z > 1)
                    return;
                _sequence[sector_z + 1, sector_x + 1] = sender.GetInt(Attributes.CellSequence);
            }
        }

        void aw_EventAvatarAdd(Instance sender)
        {
            lock (this)
            {
//                _objectLock.IsLocked = true;
                lock (sender)
                {
                    try
                    {
                        var a = AwConvert.CastAvatarObject(sender);
                        _avatar.InternalAdd(a);
                        Whisper(RoleType.debugger, "Added :" + a.Name);
                        if (AvatarEventAdd != null)
                            AvatarEventAdd.Invoke(this, new EventAvatarAddArgs(a));
                    }
                    catch (InstanceException ex)
                    {
                        HandleExceptionManaged(ex);
                    }
                }
//                _objectLock.IsLocked = false;
            }
        }

        void aw_EventObjectAdd(Instance sender)
        {
            lock (this)
            {
                Model o = null;
                lock (sender)
                {
                    try
                    {
                        // check if the object exists in the cache, this will indicat a change to the object rather than an add.
                        var result = _model.Find(p => p.Id == sender.GetInt(Attributes.ObjectId));
                        if (result == null)
                        {
                            // new object.
                            o = AwConvert.CastModelObject(sender);
                            _model.InternalAdd(o);
                            if (ObjectEventAdd != null)
                                ObjectEventAdd(this, new EventObjectAddArgs(o, GetAvatar(sender.GetInt(Attributes.AvatarSession))));
                        }
                        else
                        {
                            // existing model, add the update, aw_remove should remove the old object by its old number.
                            _model.InternalAdd(AwConvert.CastModelObject(sender));
                        }
                    }
                    catch (InstanceException ex)
                    {
                        HandleExceptionManaged(ex);
                    }
                }
            }
        }

        void aw_EventObjectDelete(Instance sender)
        {
            lock (this)
            {
                Model changedModel = null;
                if (_objectChanges.Count > 0)
                    changedModel = _objectChanges[0];

                lock (_objectChanges)
                {
                    lock (sender)
                    {
                        try
                        {
                                var remove = _model.FindAll(p => p.Id == sender.GetInt(Attributes.ObjectId));
                                if (remove.Count == 2)
                                {
                                    // this was an object update. remove index[0];
                                    _model.InternalRemove(remove[0]);
                                    if (ObjectEventChange != null)
                                    {
                                        if (changedModel != null && changedModel.Id == remove[1].Id)
                                        {
                                            // if the model exists and corelates with the object id currently removed then 
                                            // the change was a success, so we can remove the model from the change list.
                                            // note that we do not raise an event, or we will get a circular event trigger.
                                            _objectChanges.Remove(changedModel);
                                        }
                                        else
                                        {
                                            var args = new EventObjectChangeArgs(remove[1], remove[0],
                                                                                 GetAvatar(
                                                                                     sender.GetInt(
                                                                                         Attributes.AvatarSession)));
                                            ObjectEventChange(this, args);
                                        }
                                    }
                                }
                                else if (remove.Count == 1)
                                {
                                    _model.InternalRemove(remove[0]);
                                    if (ObjectEventRemove != null)
                                        ObjectEventRemove(this, new EventObjectRemoveArgs(remove[0], GetAvatar(sender.GetInt(Attributes.AvatarSession))));

                                }
                        }
                        catch (InstanceException ex)
                        {
                            HandleExceptionManaged(ex);
                        }
                    }
                    // check if there are more object changes.
                    if (_objectChanges.Count > 0)
                    {
                        changeObject(_objectChanges[0]);
                    }
                }
            }
        }

        void aw_EventObjectClick(Instance sender)
        {
            lock (this)
            {
                try
                {
                    var o = from p in _model where p.Id == sender.GetInt(Attributes.ObjectId) select p;
                    var avatar = from p in _avatar where p.Session == sender.GetInt(Attributes.AvatarSession) select p;
                    if (ObjectEventClick != null)
                        ObjectEventClick.Invoke(this, new EventObjectClickArgs(o.ElementAt(0), avatar.ElementAt(0)));
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
        }
        void aw_EventAvatarDelete(Instance sender)
        {
            lock (this)
            {
                try
                {
                    var avatar = GetAvatar(aw.GetInt(Attributes.AvatarSession));
                    // update the authorization matrix.
                    _loginConfiguration.Authorization.Matrix.RemoveAll(
                        p => p.Role == RoleType.student && p.Citizen == aw.GetInt(Attributes.AvatarCitizen));
                    _avatar.InternalRemoveAll(p => p.Session == aw.GetInt(Attributes.AvatarSession));

                    if (AvatarEventRemove != null)
                        AvatarEventRemove.Invoke(this, new EventAvatarRemoveArgs(avatar));
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
        }
        void aw_EventChat(Instance sender)
        {
            lock (this)
            {
                try
                {

                    if (ChatEvent != null)
                    {
                        var avatar = from p in _avatar where (p.Session == aw.GetInt(Attributes.ChatSession)) select p;
                        int type = aw.GetInt(Attributes.ChatType);
                        string text = aw.GetString(Attributes.ChatMessage);
                        var ct = (ChatType) type;
                        ChatEvent.Invoke(this, new EventChatArgs(avatar.ElementAt(0), ct, text));
                    }
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
        }
        void aw_EventCellObject(Instance sender)
        {
            lock (this)
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
                                        (ObjectType)aw.GetInt(Attributes.ObjectType),
                                        aw.GetString(Attributes.ObjectModel),
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
        }


        void aw_EventAvatarChange(Instance sender)
        {
            lock (this)
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
        }

        #endregion

        void AddCameraObject(Instance sender)
        {
            try
            {
                AW.Camera ret;
                sender.GetV4Object(out ret);
                var camera = new Camera() {Flags = ret.Flags, Name = ret.Name, Zoom = ret.Zoom};
                Cameras.InternalAdd(camera);
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
                AW.Mover ret;
                sender.GetV4Object(out ret);
                Movers.InternalAdd(AwConvert.CastMoverObject(ret));
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
                AW.Zone ret;
                var o = AwConvert.CastModelObject(sender);
                sender.GetV4Object(out ret);
                Zones.InternalAdd(AwConvert.CastZoneObject(ret));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        private void refresh(object o)
        {
            lock (this)
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

        #region ISceneNode Commands Members

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

        private List<Model> _objectChanges = new List<Model>();

        public void ChangeObject(Model model)
        {
            lock (this)
            {
                _objectChanges.Add(model.Clone());
                if (_objectChanges.Count == 1)
                {
                    changeObject(model);
                }
            }
        }

        private void ModelCallback(object state)
        {
            var result = (CallbackStructT<Model>)state;
            ChangeObject(result.Clone);
        }

        public void ChangeObject(Model model, int delay)
        {
            var b = new CallbackStructT<Model>(model.Clone());
            b.Timer = new Timer(ModelCallback, b, delay, 0);
        }

        private void changeObject(Model o)
        {
            lock (this)
            {
                try
                {
                    aw.SetInt(Attributes.ObjectId, o.Id);
                    aw.SetInt(Attributes.ObjectOldNumber, 0);
                    aw.SetInt(Attributes.ObjectOwner, o.Owner);
                    aw.SetInt(Attributes.ObjectType, (int) o.Type);
                    aw.SetInt(Attributes.ObjectX, (int) o.Position.x);
                    aw.SetInt(Attributes.ObjectY, (int) o.Position.y);
                    aw.SetInt(Attributes.ObjectZ, (int) o.Position.z);
                    aw.SetInt(Attributes.ObjectTilt, (int) o.Rotation.x);
                    aw.SetInt(Attributes.ObjectYaw, (int) o.Rotation.y);
                    aw.SetInt(Attributes.ObjectRoll, (int) o.Rotation.z);
                    aw.SetString(Attributes.ObjectDescription, o.Description + " " + DateTime.Now.ToLongTimeString());
                    aw.SetString(Attributes.ObjectAction, o.Action);
                    aw.SetString(Attributes.ObjectModel, o.ModelName);
                    if (o.Data != null)
                        aw.SetString(AW.Attributes.ObjectData, o.Data);
                    int rc = aw.ObjectChange();
                    if (rc != 0)
                        throw new AwException(rc);
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                    //switch (ex.ErrorCode)
                    //{
                    //    case (int) ReasonCodeReturnType.RC_TIMEOUT:
                    //        ChangeObject(o);
                    //        break;
                    //    default:
                    //        HandleExceptionManaged(ex);
                    //        break;
                    //}
                }
            }
        }

        #endregion

        #region IBaseBotEngine Members


        public bool IsEchoChat { get; set;}

        public LoginConfiguration LoginConfiguration
        {
            get { return _loginConfiguration; }
        }

        #endregion

        #region IConfigurable Members

        public IConfiguration Configuration
        {
            get
            {
                return _loginConfiguration;
            }
            set
            {
                _loginConfiguration = (LoginConfiguration)value;
            }
        }

        public IList<IConfigurable> Children { get; set; }

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

        #region ISceneNodeCommands<Model,Avatar,HudBase<Avatar>> Members

        public void HudDisplay(HudBase<Avatar> hud, Avatar avatar)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAvatarCommands<Avatar> Members

        /// <summary>
        /// Gets the avatar.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public Avatar GetAvatar(int session)
        {
            var avatar = from p in _avatar where (p.Session == session) select p;
            if (avatar.Count() == 1)
                return avatar.ElementAt(0);
            return null;
        }

        public void Teleport(Avatar avatar, Vector3 position, float yaw)
        {
            try
            {
                avatar.Position = position;
                aw.SetInt(Attributes.TeleportX, (int)position.x);
                aw.SetInt(Attributes.TeleportY, (int)position.y);
                aw.SetInt(Attributes.TeleportZ, (int)position.z);
                aw.SetInt(Attributes.TeleportYaw, (int)yaw);
                aw.Teleport(avatar.Session);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        public string GetCizitenNameByNumber(int citizen)
        {
            if (citizen == 0)
                return "root";

            var avatar = _avatar.Find(p => p.Citizen == citizen);
            int rc = 0;
            if (avatar != null)
                return avatar.Name;
            try
            {
                rc = aw.CitizenAttributesByNumber(citizen);
            }
            catch (InstanceException ex)
            {
                // supress error. this is not properly implemented by the Aw SDK wrapper of bytemr?
            }
            finally
            {
                if (rc != 0)
                    throw new AwException(rc);
            }
            var name = aw.GetString(Attributes.CitizenName);
            if (name == string.Empty)
                name = citizen.ToString();
            Utility.Wait(3000);
            return name;
        }



        public void Teleport(Avatar avatar, float x, float y, float z, float yaw)
        {
            Teleport(avatar, new Vector3(x, y, z), yaw);
        }

        #endregion

        #region IBaseBotEngine Members

        public bool IsEnterGlobal { get; private set; }

        #endregion

        #region IHandleExceptionManaged Members

        public void HandleExceptionManaged(int rc)
        {
            throw new AwException(rc);
            //TODO: handle universe server diconnected exception for example. keep the bot alive.
            //TODO: Handle server disconnected exception.
        }

        public void HandleExceptionManaged(InstanceException instanceException)
        {

          //  throw new AwException(instanceException.ErrorCode);
            //TODO: handle universe server diconnected exception for example. keep the bot alive.
            //TODO: Handle server disconnected exception.
        }

        #endregion

        #region IChatCommands<Avatar> Members

        public void Whisper(RoleType role, string message)
        {
            foreach (var a in from p in _avatar select p)
            {
                if (_loginConfiguration.Authorization.IsInRole(role, a.Citizen))
                {
                    aw.Whisper(a.Session, message);
                }
                var l = new List<string>();
            }
        }
        public void Whisper(Avatar avatar, string message)
        {
            try
            {
                aw.Whisper(avatar.Session, message);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }
        public void Say(string message)
        {
            try
            {
                aw.Say(message);
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }
 
        public void Say(int delay, SessionArgumentType argumentType, Avatar avatar, string message)
        {
            var o = new CallbackStructT<Avatar>(avatar.Clone(),new object[]{message,argumentType});
            o.Timer = new Timer(SayCallback, o, delay, 0);
        }

        private void SayCallback(object state)
        {
            var result = (CallbackStructT<Avatar>) state;
            var exists = _avatar.Exists(p => p.Session == result.Clone.Session);
            switch ((SessionArgumentType)result.Param[1])
            {
                // only send a message if the session is stil available.
                case SessionArgumentType.AvatarSessionMustExist:
                    if (exists)
                        Say(result.Param[0].ToString());
                    break;
                // only send a message if the session os not available.
                case SessionArgumentType.AvatarSessionMustNotExist:
                    if (!exists)
                        Say(result.Param[0].ToString());
                    break;
            }
        }

        #endregion
    }
}