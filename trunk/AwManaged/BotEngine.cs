﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.Converters;
using AwManaged.Core;
using AwManaged.Core.Interfaces;
using AwManaged.EventHandling;
using AwManaged.ExceptionHandling;
using AwManaged.Interfaces;
using AwManaged.Logging;
using AwManaged.Math;
using AwManaged.Scene;
using AWManaged.Security;
using AwManaged.Storage;
using AwManaged.Storage.Interfaces;
using Camera=AwManaged.Scene.Camera;
using Mover=AwManaged.Scene.Mover;
using Zone=AwManaged.Scene.Zone;

namespace AwManaged
{
    /// <summary>
    /// Exact abstract type implementation of an active worlds "master" bot.
    /// </summary>
    public abstract class BotEngine : IBotEngine<Avatar,Model,Camera,Zone,Mover,HudBase<Avatar>>
    {
        #region Fields
        private LoginConfiguration _universeConnection;
        private Instance aw { get; set; }
        private Timer _timer;
        //private ProtectedList<Model> _model = new ProtectedList<Model>();
        private Model _modelRemoved;

        //private ProtectedList<Camera> _cameras = new ProtectedList<Camera>();
        //private ProtectedList<Mover> _movers = new ProtectedList<Mover>();
        //private ProtectedList<Zone> _zones = new ProtectedList<Zone>();
        //private ProtectedList<HudBase<Avatar>> _huds = new ProtectedList<HudBase<Avatar>>();
        //private ProtectedList<Avatar> _avatar = new ProtectedList<Avatar>();

        private IStorageServer<Db4OConnection> _storageServer; // TODO: expose as interface in IBotEngine
        private SceneNodes _sceneNodes = new SceneNodes();
        #endregion

        #region IBotEngine<Avatar,Model,Camera,Zone,Mover,HudBase<Avatar>> Members

        /// <summary>
        /// Gets a cloned version of the scene nodes. Avoid using this as it is both time and memory consuming.
        /// </summary>
        /// <value>The scene nodes.</value>
        public AwManaged.Scene.Interfaces.ISceneNodes<Model, Camera, Mover, Zone, HudBase<Avatar>, Avatar> SceneNodes
        {
            get { lock (this){return _sceneNodes.Clone();}}
        }

        #region Properties

////        public SceneNodes.SceneNodes SceneNodes{get{lock (this){return _sceneNodes.Clone();}}}
//        public ProtectedList<Model> Models { get { lock (this){return _model.Clone();} } }
//        public ProtectedList<Camera> Cameras { get { lock(this){ return _cameras.Clone();} } }
//        public ProtectedList<Mover> Movers { get { lock (this){return _movers.Clone();} } }
//        public ProtectedList<Zone> Zones { get { lock (this){return _zones.Clone();} } }
//        public ProtectedList<HudBase<Avatar>> Huds { get { lock (this){return _huds.Clone();} } }

        private List<IStorageClient<Db4OConnection>> _storage = new List<IStorageClient<Db4OConnection>>();

        public IStorageClient<Db4OConnection> Storage
        {
            get;
            private set;
        } // TODO: expose as interface in IBotEngine.

        #endregion

        #region Delegates and Events

        public event BotEventLoggedInDelegate<BotEngine,UniverseConnectionProperties> BotEventLoggedIn;
        public event BotEventEntersWorldDelegate<BotEngine,UniverseConnectionProperties> BotEventEntersWorld;
        public event AvatarEventAddDelegate<BotEngine,Avatar> AvatarEventAdd;
        public event AvatarEventChangeDelegate<BotEngine,Avatar> AvatarEventChange;
        public event AvatarEventRemoveDelegate<BotEngine,Avatar> AvatarEventRemove;
        public event ObjectEventClickDelegate<BotEngine,Avatar,Model> ObjectEventClick;
        public event ObjectEventAddDelegate<BotEngine,Avatar,Model> ObjectEventAdd;
        public event ObjectEventRemoveDelegate<BotEngine,Avatar,Model> ObjectEventRemove;
        public event ObjectEventScanCompletedDelegate<BotEngine,SceneNodes> ObjectEventScanCompleted;
        public event ChatEventDelegate<BotEngine,Avatar> ChatEvent;
        public event ObjectEventChangeDelegate<BotEngine,Avatar,Model> ObjectEventChange;

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

            WriteLine("Scan completed, found " + _sceneNodes.Models.Count + " objects in " + LoginConfiguration.Connection.World + ".");
            _timer = new Timer(refresh, null, 0, 10);

            if (ObjectEventScanCompleted != null)
                ObjectEventScanCompleted.Invoke(this, new EventObjectScanCompletedEventArgs<SceneNodes>(_sceneNodes));

        }

        #endregion

        #region ScanObjects Global

        public virtual void ScanObjects()
        {
            try
            {
                _sceneNodes.Models = new ProtectedList<Model>();

                WriteLine("Scanning objects in world" + LoginConfiguration.Connection.World + ".");

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

                WriteLine("Scan completed, found " + _sceneNodes.Models.Count + " objects in " + LoginConfiguration.Connection.World + ".");

                _timer = new Timer(refresh, null, 0, 10);
                if (ObjectEventScanCompleted != null)
                    ObjectEventScanCompleted.Invoke(this, new EventObjectScanCompletedEventArgs<SceneNodes>(_sceneNodes));
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
                aw = new Instance(_universeConnection.Connection.Domain, _universeConnection.Connection.Port);
                //greeter
                //Set the login attributes and log the bot into the universe
                aw.SetString(Attributes.LoginName, _universeConnection.Connection.LoginName);
                aw.SetString(Attributes.LoginPrivilegePassword, _universeConnection.Connection.PrivilegePassword);
                aw.SetInt(Attributes.LoginOwner, _universeConnection.Connection.Owner);
                rc = aw.Login();
                if (rc != 0)
                    HandleExceptionManaged(rc);

                if (BotEventLoggedIn!=null)
                    BotEventLoggedIn(this, new EventBotLoggedInArgs<UniverseConnectionProperties>(LoginConfiguration.Connection));

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
                    rc = aw.Enter(_universeConnection.Connection.World);
                    IsEnterGlobal = true;
                    if (rc != 0)
                        HandleExceptionManaged(rc);
                }
                else
                {
                    aw.SetBool(Attributes.EnterGlobal, false);
                    IsEnterGlobal = false;
                    rc = aw.Enter(_universeConnection.Connection.World);
                    if (rc != 0)
                    {
                        HandleExceptionManaged(rc);
                    }
                }

                if (BotEventEntersWorld != null)
                    BotEventEntersWorld.Invoke(this, new EventBotEntersWorldArgs<UniverseConnectionProperties>(LoginConfiguration.Connection));

                //Have the bot change state to 0n 0w 0a
                aw.SetInt(Attributes.MyX, (int)_universeConnection.Connection.Position.x); //X position of the bot (E/W)
                aw.SetInt(Attributes.MyY, (int)_universeConnection.Connection.Position.y); //Y position of the bot (height)
                aw.SetInt(Attributes.MyZ, (int)_universeConnection.Connection.Position.z); //Z position of the bot (N/S)

                aw.SetInt(Attributes.MyPitch, (int)_universeConnection.Connection.Position.y);
                aw.SetInt(Attributes.MyYaw, (int)_universeConnection.Connection.Position.z);

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

        //[Obsolete("Please use loginConfiguration database.")]
        //protected BotEngine(Authorization authorization, string domain, int port, int loginOwner, string privilegePassword, string loginName, string world, Vector3 position, Vector3 rotation)
        //{
        //    _loginConfiguration = new LoginConfiguration(authorization, domain, port, loginOwner, privilegePassword, loginName, world, position, rotation);
        //    aw = new Instance(_loginConfiguration.Domain, _loginConfiguration.Port);
        //}
        
        ////protected BotEngine(string configurationName)
        ////{
        ////    var configuration = new LoginConfiguration(configurationName);
        ////}

        protected BotEngine(LoginConfiguration loginConfiguration, IConnection<Db4OConnection> storageConfiguration)
        {
            _universeConnection = loginConfiguration;
            _storageServer = new Db4OStorageServer(storageConfiguration);
            _storageServer.Start();
            // start the database client.
            Storage = new Db4OStorageClient(storageConfiguration);
            Storage.OpenConnection();
        }

        protected BotEngine(LoginConfiguration loginConfiguration)
        {
            _universeConnection = loginConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BotEngine"/> class.
        /// Load the configuration from the App.Config
        /// </summary>
        protected BotEngine()
        {
            _universeConnection = new LoginConfiguration(ConfigurationManager.AppSettings["UniverseConnection"]);
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["StorageServerConnection"]))
            {
                // only create a storage server if we have specified this in the config, otherwise we are using a remote server.
                _storageServer =new Db4OStorageServer(new StorageConfiguration<Db4OConnection>(ConfigurationManager.AppSettings["StorageServerConnection"]));
                if (!_storageServer.Start())
                    throw new Exception("Can't start the storage server.");
            }

            Storage = new Db4OStorageClient(new StorageConfiguration<Db4OConnection>(ConfigurationManager.AppSettings["StorageClientConnection"]));
            Storage.OpenConnection();
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
                        _sceneNodes.Avatars.InternalAdd(a);
                        Whisper(RoleType.debugger, "Added :" + a.Name);
                        if (AvatarEventAdd != null)
                            AvatarEventAdd.Invoke(this, new EventAvatarAddArgs<Avatar>(a));
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
                switch (aw.GetInt(Attributes.ObjectType))
                {
                    case (int)AW.ObjectType.Camera:
                        AddCameraObject(sender);
                        return;
                    case (int)AW.ObjectType.Mover:
                        AddMoverObject(sender);
                        return;
                    case (int)AW.ObjectType.Zone:
                        aw_addZoneObject(sender);
                        return;
                }
                Model o = null;


                lock (sender)
                {
                    try
                    {
                        // check if the object exists in the cache, this will indicat a change to the object rather than an add.
                        var result = _sceneNodes.Models.Find(p => p.Id == sender.GetInt(Attributes.ObjectId));
                        if (result == null)
                        {
                            // new object.
                            o = AwConvert.CastModelObject(sender);
                            _sceneNodes.Models.InternalAdd(o);
                            if (ObjectEventAdd != null)
                                ObjectEventAdd(this, new EventObjectAddArgs<Avatar,Model>(o, GetAvatar(sender.GetInt(Attributes.AvatarSession))));
                        }
                        else
                        {
                            // existing model, add the update, aw_remove should remove the old object by its old number.
                            SceneNodes.Models.InternalAdd(AwConvert.CastModelObject(sender));
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
                                var remove = _sceneNodes.Models.FindAll(p => p.Id == sender.GetInt(Attributes.ObjectId));
                                if (remove.Count == 2)
                                {
                                    // this was an object update. remove index[0];
                                    _sceneNodes.Models.InternalRemove(remove[0]);
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
                                            var args = new EventObjectChangeArgs<Avatar,Model>(remove[1], remove[0],
                                                                                 GetAvatar(
                                                                                     sender.GetInt(
                                                                                         Attributes.AvatarSession)));
                                            ObjectEventChange(this, args);
                                        }
                                    }
                                }
                                else if (remove.Count == 1)
                                {
                                    _sceneNodes.Models.InternalRemove(remove[0]);
                                    if (ObjectEventRemove != null)
                                        ObjectEventRemove(this, new EventObjectRemoveArgs<Avatar,Model>(remove[0], GetAvatar(sender.GetInt(Attributes.AvatarSession))));

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
                    var o = from p in _sceneNodes.Models where p.Id == sender.GetInt(Attributes.ObjectId) select p;
                    var avatar = from p in _sceneNodes.Avatars where p.Session == sender.GetInt(Attributes.AvatarSession) select p;
                    if (ObjectEventClick != null)
                        ObjectEventClick.Invoke(this, new EventObjectClickArgs<Avatar,Model>(o.ElementAt(0), avatar.ElementAt(0)));
                }
                catch (InstanceException ex)
                {
                    HandleExceptionManaged(ex);
                }
            }
            Utility.Wait(0);
        }
        void aw_EventAvatarDelete(Instance sender)
        {
            lock (this)
            {
                try
                {
                    var avatar = GetAvatar(aw.GetInt(Attributes.AvatarSession));
                    // update the authorization matrix.
                    _universeConnection.Connection.Authorization.Matrix.RemoveAll(
                        p => p.Role == RoleType.student && p.Citizen == aw.GetInt(Attributes.AvatarCitizen));
                    _sceneNodes.Avatars.InternalRemoveAll(p => p.Session == aw.GetInt(Attributes.AvatarSession));

                    if (AvatarEventRemove != null)
                        AvatarEventRemove.Invoke(this, new EventAvatarRemoveArgs<Avatar>(avatar));
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
                        var avatar = from p in _sceneNodes.Avatars where (p.Session == aw.GetInt(Attributes.ChatSession)) select p;
                        int type = aw.GetInt(Attributes.ChatType);
                        string text = aw.GetString(Attributes.ChatMessage);
                        var ct = (ChatType) type;
                        ChatEvent.Invoke(this, new EventChatArgs<Avatar>(avatar.ElementAt(0), ct, text));
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
                        aw_addZoneObject(sender);
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

                    _sceneNodes.Models.InternalAdd(o);
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
                var avatar = from p in _sceneNodes.Avatars where p.Session == aw.GetInt(Attributes.AvatarSession) select p;

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
                _sceneNodes.Cameras.InternalAdd(camera);
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
                _sceneNodes.Movers.InternalAdd(AwConvert.CastMoverObject(ret));
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }
        void aw_addZoneObject(Instance sender)
        {
            try
            {
                AW.Zone ret;
                sender.GetV4Object(out ret);
                var zone = AwConvert.CastZoneObject(ret);
                zone.Model = AwConvert.CastModelObject(sender);
                _sceneNodes.Models.InternalAdd(zone.Model);
                _sceneNodes.Zones.InternalAdd(zone);
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
                _storageServer.Stop();
                Storage.CloseConnection();
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
            get { return _universeConnection; }
        }

        #endregion

        #region IConfigurable Members

        public IConfiguration Configuration
        {
            get
            {
                return _universeConnection;
            }
            set
            {
                _universeConnection = (LoginConfiguration)value;
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
            var avatar = from p in _sceneNodes.Avatars where (p.Session == session) select p;
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

            var avatar = _sceneNodes.Avatars.Find(p => p.Citizen == citizen);
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
            foreach (var a in from p in _sceneNodes.Avatars select p)
            {
                if (_universeConnection.Connection.Authorization.IsInRole(role, a.Citizen))
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
            var exists = _sceneNodes.Avatars.Exists(p => p.Session == result.Clone.Session);
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

 #endregion
    }
}