/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.Configuration.Interfaces;
using AwManaged.ConsoleServices;
using AwManaged.Converters;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Patterns;
using AwManaged.Core.Scheduling;
using AwManaged.Core.ServicesManaging;
using AwManaged.Core.ServicesManaging.Interfaces;
using AwManaged.EventHandling;
using AwManaged.EventHandling.BotEngine; /* Use event handlers for exact implementation of the botengine */
using AwManaged.ExceptionHandling;
using AwManaged.Interfaces;
using AwManaged.LocalServices;
using AwManaged.LocalServices.WebServer;
using AwManaged.Logging;
using AwManaged.Math;
using AwManaged.RemoteServices;
using AwManaged.RemoteServices.Server;
using AwManaged.Scene;
using AwManaged.Scene.ActionInterpreter.Attributes;
using AwManaged.Scene.ActionInterpreter.Interface;
using AWManaged.Security;
using AwManaged.Security.RemoteBotEngine;
using AwManaged.Storage;
using Camera=AwManaged.Scene.Camera;
using Mover=AwManaged.Scene.Mover;
using Zone=AwManaged.Scene.Zone;

namespace AwManaged
{
    /// <summary>
    /// Exact abstract type implementation of an active worlds "master" bot.
    /// </summary>
    public class BotEngine : MarshalByRefObject, IBotEngine<Avatar,Model,Camera,Zone,Mover,HudBase<Avatar>,Scene.Particle,Scene.ParticleFlags, Db4OConnection, LocalBotPluginServicesManager>
    {


        public ConsoleHelpers Console = new ConsoleHelpers();

        #region Fields
        private const int _maxFlushItems = 255;
        private LoginConfiguration _universeConnection;
        private Instance aw { get; set; }
        private Timer _timer;
        private Model _modelRemoved;
        private SceneNodes _sceneNodes = new SceneNodes();
        private SceneNodes _sceneNodesNew = new SceneNodes();
        private Db4OStorageClient _storageClient;
        private Db4OStorageClient _authStorageClient;
        private Db4OStorageServer _storageServer;
        private Db4OStorageServer _authStorageServer;
        private GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute> _actionInterpreter;
        private int BotHash = Guid.NewGuid().GetHashCode();
        private WebServerService _webServerService;
        private int Nodes = 0;
        private int CurrentNode;
        private List<BotNode<UniverseConnectionProperties, Model>> BotNodes = new List<BotNode<UniverseConnectionProperties, Model>>();

        #endregion

        [Browsable(false)]
        public Db4OStorageClient Storage { get { return _storageClient; } }
        [Browsable(false)]
        public SchedulingService SchedulingService;

        #region IBotEngine<Avatar,Model,Camera,Zone,Mover,HudBase<Avatar>> Members

        /// <summary>
        /// Gets a cloned version of the scene nodes. Avoid using this as it is both time and memory consuming.
        /// </summary>
        /// <value>The scene nodes.</value>
        [Browsable(false)]
        public AwManaged.Scene.Interfaces.ISceneNodes<Model, Camera, Mover, Zone, HudBase<Avatar>, Avatar, Scene.Particle, Scene.ParticleFlags> SceneNodes
        {
            get { lock (this){return _sceneNodes.Clone();}}
        }

        #region Delegates and Events

        public event BotEventSlaveStarted BotEventSlaveStarted;
        public event BotEventLoggedInDelegate BotEventLoggedIn;
        public event BotEventEntersWorldDelegate BotEventEntersWorld;
        public event AvatarEventAddDelegate AvatarEventAdd;
        public event AvatarEventChangeDelegate AvatarEventChange;
        public event AvatarEventRemoveDelegate AvatarEventRemove;
        public event ObjectEventClickDelegate ObjectEventClick;
        public event ObjectEventAddDelegate ObjectEventAdd;
        public event ObjectEventRemoveDelegate ObjectEventRemove;
        public event ObjectEventScanCompletedDelegate ObjectEventScanCompleted;
        public event ChatEventDelegate ChatEvent;
        public event ObjectEventChangeDelegate ObjectEventChange;
        public event TransactionEventCompletedDelegate TransactionEventCompleted;

        #endregion

        #region IBaseBotEngine Implementation

        #region ScanObjects Non Global

        readonly int[,] _sequence = new int[3, 3];
        int _queryX;
        int _queryZ;

        public Version Version()
        {
            Assembly asm = Assembly.GetAssembly(typeof(BotEngine));
            string fullVersionSpec = asm.FullName.Split(',')[1];
            return new Version(fullVersionSpec.Substring(fullVersionSpec.IndexOf('=') + 1));
        }


        /// <summary>
        /// Scans the objects non global. incase the bot is not running under Care Taker authorization.
        /// </summary>
        private void ScanObjectsNonGlobal()
        {
            lock (this)
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

                WriteLine("Scan completed, found " + _sceneNodes.Models.Count + " objects in " +
                          LoginConfiguration.Connection.World + ".");
                Scancompleted();
            }
        }

        #endregion

        private void Scancompleted()
        {
            lock (this)
            {
                _timer = new Timer(refresh, null, 0, 10);
                if (ObjectEventScanCompleted != null)
                    ObjectEventScanCompleted.Invoke(this, new EventObjectScanCompletedEventArgs(_sceneNodes));

                for (int node = 0; node < Nodes; node++)
                {
                    var botnode = new BotNode<UniverseConnectionProperties, Model>(_universeConnection.Connection, node);
                    botnode.Connect();
                    botnode.Login();
                    BotNodes.Add(botnode);
                    if (BotEventSlaveStarted != null)
                        BotEventSlaveStarted(this, new EventBotSlaveStartedArgs(botnode.Connection,node));
                }
            }
        }

        #region ScanObjects Global

        /// <summary>
        /// Scan's objects in either global or non global mode. (auto sensing mode)
        /// </summary>
        public void ScanObjects()
        {
            try
            {
                _sceneNodes.Models = new ProtectedList<Model>();

                WriteLine("Scanning objects in world" + LoginConfiguration.Connection.World + ".");
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
                Scancompleted();
            }
            catch (InstanceException ex)
            {
                HandleExceptionManaged(ex);
            }
        }

        #endregion

        private SimpleTransaction<Model> _transaction = new SimpleTransaction<Model>(null);


        public void AddObjects(SimpleTransaction<Model> transaction)
        {
            lock (this)
            {
                if ((_transaction == null || _transaction.CommitsPending == 0) ||
                    (_transaction != null && _transaction.CommitsPending == 0))
                {
                    _transaction = transaction;
                    _transaction.Commit();
                    for (int i = 0; i < _transaction._transactionList.Count();i++ )
                    {
                        AddObject(_transaction._transactionList[i]);
                    }
                    return;
                }
                throw new Exception("Right now, we support only one transaction at a time.");
            }
        }

        public void AddObject(Model o)
        {
            // prevent disconnection from the uni on massive updates.
            while (_pendingTransactions > 255)
                Utility.Wait(10);
            lock (this)
            {
                if (o.Hash == 0) o.Hash = Guid.NewGuid().GetHashCode();
                o.TransactionItemType = TransactionItemType.Add;
                o.IsRuntimeTransaction = true;
                _sceneNodesNew.Models.InternalAdd(o.Clone());
                AwConvert.SetObject(aw,o);
                _pendingTransactions++;
                aw.SetInt(Attributes.ObjectCallbackReference, o.Hash);
                int rc = aw.ObjectAdd();
                if (rc!=0)
                    throw new AwException(rc);
            }
        }

        public void ChangeObject(Model model)
        {
            // prevent disconnection from the uni on massive updates.
            while (_pendingTransactions > 255)
                Utility.Wait(10);
            lock (this)
            {
                if (model.Hash == 0) model.Hash = Guid.NewGuid().GetHashCode();
                model.TransactionItemType = TransactionItemType.Add;
                model.IsRuntimeTransaction = true;
                _sceneNodesNew.Models.InternalAdd(model.Clone());
                AwConvert.SetObject(aw, model);
                _pendingTransactions++;
                aw.SetInt(Attributes.ObjectCallbackReference, model.Hash);
                int rc = aw.ObjectChange();
                if (rc != 0)
                    throw new AwException(rc);
            }
        }

        public void DeleteObject(Model o)
        {
            // prevent disconnection from the uni on massive updates.
            while (_pendingTransactions > 255)
                Utility.Wait(10);
            lock (this)
            {
                if (o.Hash == 0) o.Hash = Guid.NewGuid().GetHashCode();
                o.TransactionItemType = TransactionItemType.Remove;
                o.IsRuntimeTransaction = true;
                _sceneNodesNew.Models.InternalAdd(o.Clone());
                aw.SetInt(Attributes.ObjectId, o.Id);
                aw.SetInt(Attributes.ObjectNumber, 0);
                aw.SetInt(Attributes.ObjectCallbackReference, o.Hash);
                _pendingTransactions++;
                int rc = aw.ObjectDelete();
                if (rc != 0)
                    throw new AwException(rc);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="error">The error.</param>
        void aw_CallbackObjectResult(Instance sender, int error)
        {
            lock (this)
            {
                Model o;
                var hash = sender.GetInt(Attributes.ObjectCallbackReference);
                if (hash != 0)
                {
                    o = _sceneNodesNew.Models.Find(p => p.Hash == hash);
                    _sceneNodesNew.Models.InternalRemove(o);
                    if (o == null)
                        o = _transaction._transactionList.Find(p => p.Hash == hash);
                    if (o!=null && o.TransactionId != 0)
                    {
                        o.Id = sender.GetInt(Attributes.ObjectId);
                        var model = _transaction._transactionList.Find(p => p.Hash == hash);
                        if (model != null)
                        {
                            _transaction.Commit(model);
                        }
                    }
                    _sceneNodes.Models.InternalAdd(o);
                }
                else
                {
                    throw new Exception("Hash not set.");
                }
                if (_pendingTransactions > 0)
                    _pendingTransactions--;
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
                var id = sender.GetInt(Attributes.ObjectId);
                var o = _sceneNodes.Models.Find(p => p.Id == id);
                if (o == null) // new object added outside this runtime.
                {
                    o = AwConvert.CastModelObject(sender);
                    o.TransactionItemType = TransactionItemType.Add;
                    _sceneNodes.Models.InternalAdd(o);
                    if (ObjectEventAdd != null)
                        ObjectEventAdd(this, new EventObjectAddArgs(o, GetAvatarByCitnum(o.Owner)));
                }

                if (_transaction._finished)
                    _transaction.Completed();
            }
        }

        /// <summary>
        /// Handels the aw delete, mainly process the _objectChanges and _objectDeletes queue.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void aw_EventObjectDelete(Instance sender)
        {
            lock (this)
            {
                var id = sender.GetInt(Attributes.ObjectId);
                var o = _sceneNodes.Models.FindAll(p => p.Id == id);
                if (o.Count == 2) // change, last record is winner.
                {
                    if (!o[1].IsRuntimeTransaction)
                    {
                        if (ObjectEventChange != null)
                            ObjectEventChange(this, new EventObjectChangeArgs(o[1],o[0], GetAvatarByCitnum(o[1].Owner)));
                    }
                }
                else if (!o[0].IsRuntimeTransaction) // new object added outside this runtime.
                {
                    if (ObjectEventRemove != null)
                        ObjectEventRemove(this, new EventObjectRemoveArgs(o[0], GetAvatarByCitnum(o[0].Owner)));
                }

                _sceneNodes.Models.InternalRemove(o[0]);

                if (_transaction._finished)
                    _transaction.Completed();
            }
        }

        private TimeSpan _vrtTimeDifference;

        public DateTime VrtTime()
        {
            return DateTime.Now.Add(_vrtTimeDifference);
        }

        public void Start()
        {
            StartServices();
            try
            {
                int rc;

                aw = AwHelpers.AwHelpers.Connect(_universeConnection.Connection,false);
                aw.EventAvatarAdd += aw_EventAvatarAdd;
                AwHelpers.AwHelpers.Login(aw, _universeConnection.Connection, false);


                var time = AwConvert.ConvertFromUnixTimestamp(aw.GetInt(Attributes.UniverseTime)).AddHours(-2);
                _vrtTimeDifference = (time - DateTime.Now);
                

                if (BotEventLoggedIn!=null)
                    BotEventLoggedIn(this, new EventBotLoggedInArgs(LoginConfiguration.Connection,0));

                aw.EventAvatarChange += aw_EventAvatarChange;
                aw.EventChat += aw_EventChat;
                aw.EventAvatarDelete += aw_EventAvatarDelete;
                aw.EventObjectClick += aw_EventObjectClick;
                aw.EventObjectDelete += aw_EventObjectDelete;
                aw.EventObjectAdd += aw_EventObjectAdd;
                // immedia result.
                aw.CallbackObjectResult += aw_CallbackObjectResult;

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
                    BotEventEntersWorld.Invoke(this, new EventBotEntersWorldArgs(LoginConfiguration.Connection));

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

        private int _pendingTransactions;

        #endregion

        #region Constructors

        //protected BotEngine(LoginConfiguration loginConfiguration, IConnection<Db4OConnection> storageConfiguration)
        //{
        //    _universeConnection = loginConfiguration;
        //    _storageServer = new Db4OStorageServer(storageConfiguration);
        //    _storageServer.Start();
        //    // start the database client.
        //    Storage = new Db4OStorageClient(storageConfiguration);
        //    Storage.OpenConnection();
        //}

        //protected BotEngine(LoginConfiguration loginConfiguration)
        //{
        //    _universeConnection = loginConfiguration;
        //}

        public IEnumerable<ICommandGroup> Interpret(string action)
        {
            try
            {
                return _actionInterpreter.Interpret(action);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ConsoleMessageType.Error, ex.Message);
                return null;
            }
        }

        private void StartServices()
        {
            SchedulingService = new SchedulingService();
            ServicesManager.Start();
            LocalBotPluginServicesManager.Start();
            ServicesManager.AddService(SchedulingService);

            _actionInterpreter = new GenericInterpreterService<ACEnumTypeAttribute, ACEnumBindingAttribute, ACItemBindingAttribute>(Assembly.GetAssembly(typeof(ACEnumBindingAttribute))) { IdentifyableTechnicalName = "ActionInterpreter" };
            ServicesManager.AddService(_actionInterpreter);
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UniverseConnection"]))
                throw new Exception("No universe connection specified in your app.config.");
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["StorageClientConnection"]))
                throw new Exception("No storage client connection specified in your app.config.");

            _universeConnection = new LoginConfiguration(ConfigurationManager.AppSettings["UniverseConnection"]);

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["StorageServerConnection"]))
            {
                _storageServer = new Db4OStorageServer(ConfigurationManager.AppSettings["StorageServerConnection"]){IdentifyableTechnicalName = "StorageServerConnection"};
                ServicesManager.AddService(_storageServer);
            }

            _storageClient = new Db4OStorageClient(ConfigurationManager.AppSettings["StorageClientConnection"]) { IdentifyableTechnicalName = "StorageClientConnection" };
            ServicesManager.AddService(_storageClient);


            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuthStorageServerConnection"]))
            {
                _authStorageServer = new Db4OStorageServer(ConfigurationManager.AppSettings["AuthStorageServerConnection"]){IdentifyableTechnicalName = "AuthStorageServerConnection"};
                ServicesManager.AddService(_authStorageServer);
            }

            if (!String.IsNullOrEmpty("AuthStorageClientConnection"))
            {
                _authStorageClient = new Db4OStorageClient(ConfigurationManager.AppSettings["AuthStorageClientConnection"]){IdentifyableTechnicalName = "AuthStorageClientConnection"};
                ServicesManager.AddService(_authStorageClient);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["WebServerConnection"]))
            {
                _webServerService = new WebServerService(ConfigurationManager.AppSettings["WebServerConnection"]){IdentifyableTechnicalName="w3svc"};
                ServicesManager.AddService(_webServerService);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RemotingServerConnection"]))
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuthStorageClientConnection"]))
                    throw new Exception("To successfully run a remoting server connection, there must be an authentication storage client connection available.");
                var idm = new IdmClient<Db4OConnection>(_authStorageClient);
                _remotingServer = new RemotingServer<RemotingBotEngine>(ConfigurationManager.AppSettings["RemotingServerConnection"],idm) { IdentifyableTechnicalName = "RemotingServerConnection" };
                ServicesManager.AddService(_remotingServer);
            }

            ServicesManager.StartServices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BotEngine"/> class.
        /// Load the configuration from the App.Config
        /// </summary>
        public BotEngine()
        {
            ServicesManager = new ServicesManager();
            LocalBotPluginServicesManager = new LocalBotPluginServicesManager(this);
        }


        private RemotingServer<RemotingBotEngine> _remotingServer;

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
        private bool IsLocalChange(int callbackHash)
        {
            if (callbackHash == BotHash || _transaction.ContainsHash(callbackHash))
                return true;
            return false;
        }
        private Avatar GetAvatarByCitnum(int citnum)
        {
            var avatar= _sceneNodes.Avatars.Find(p => p.Citizen == citnum);
            if (avatar == null)
                return new Avatar() {Citizen = citnum};
            else
                return avatar;
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
                        ObjectEventClick(this, new EventObjectClickArgs(o.ElementAt(0), avatar.ElementAt(0)));
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
                        var avatar = from p in _sceneNodes.Avatars where (p.Session == aw.GetInt(Attributes.ChatSession)) select p;
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
                        aw_addZoneObject(sender);
                        return;
                    case (int)AW.ObjectType.Particle:
                        aw_addParticleObject(sender);
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

                    string trans = aw.GetString(Attributes.ObjectCallbackReference);
                    if (trans !=null)
                    {
                        
                    }

                    Model o = new Model(aw.GetInt(Attributes.ObjectId),
                                        aw.GetInt(Attributes.ObjectOwner),
                                        AwConvert.ConvertFromUnixTimestamp(aw.GetInt(Attributes.ObjectBuildTimestamp)),
                                        (ObjectType)aw.GetInt(Attributes.ObjectType),
                                        aw.GetString(Attributes.ObjectModel),
                                        position, rotation, aw.GetString(Attributes.ObjectDescription),
                                        aw.GetString(Attributes.ObjectAction)/*, aw.GetInt(Attributes.ObjectNumber) 
                                        /*aw.GetString(Attributes.ObjectData)*/);
                    o.TransactionItemType = TransactionItemType.Scan;
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
        private void aw_addParticleObject(Instance sender)
        {
            lock(this)
            {
                try
                {
                    AW.Particle ret;
                    sender.GetV4Object(out ret);
                    _sceneNodes.Particles.InternalAdd(AwConvert.CastParticleObject(ret));
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

        private void SetTracking(Model o, TransactionItemType transactionType)
        {
            o.Hash = Guid.NewGuid().GetHashCode();
            o.TransactionItemType = transactionType;
        }


        private List<Model> _objectChanges = new List<Model>();
        private List<Model> _objectDeletes = new List<Model>();

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

        public delegate void AsyncChangeObject(Model o, ObjectTransactionType type);


        //private void changeObject(Model o)
        //{
        //    lock (this)
        //    //{
        //        try
        //        {
        //                //if (CurrentNode >= Nodes)
        //                //{
        //                //    CurrentNode = 0;
        //                //}

        //                // indicate which node session should handle the change to the object.
        //                //o._ChangeNode = CurrentNode;
        //                //AsyncChangeObject inv = BotNodes[CurrentNode].ObjectTransaction;
        //                //inv.BeginInvoke(o, ObjectTransactionType.Change, null, null);
        //                //CurrentNode++;
        //                aw.SetInt(Attributes.ObjectId, o.Id);
        //                aw.SetInt(Attributes.ObjectOldNumber, 0);
        //                aw.SetInt(Attributes.ObjectOwner, o.Owner);
        //                aw.SetInt(Attributes.ObjectType, (int)o.Type);
        //                aw.SetInt(Attributes.ObjectX, (int)o.Position.x);
        //                aw.SetInt(Attributes.ObjectY, (int)o.Position.y);
        //                aw.SetInt(Attributes.ObjectZ, (int)o.Position.z);
        //                aw.SetInt(Attributes.ObjectTilt, (int)o.Rotation.x);
        //                aw.SetInt(Attributes.ObjectYaw, (int)o.Rotation.y);
        //                aw.SetInt(Attributes.ObjectRoll, (int)o.Rotation.z);
        //                aw.SetString(Attributes.ObjectDescription, o.Description);
        //                aw.SetString(Attributes.ObjectAction, o.Action);
        //                //aw.SetString(Attributes.ObjectModel, o.ModelName);
        //                //if (o.Data != null)
        //                //    aw.SetString(AW.Attributes.ObjectData, o.Data);
        //                int rc = aw.ObjectChange();
        //                if (rc != 0)
        //                    throw new AwException(rc);
        //                _sceneNodes.Models.InternalRemoveAll(p => p.Id == o.Id);
        //                _sceneNodes.Models.InternalAdd(o);
        //            }
        //            catch (InstanceException ex)
        //            {
        //                HandleExceptionManaged(ex);
        //                //switch (ex.ErrorCode)
        //                //{
        //                //    case (int) ReasonCodeReturnType.RC_TIMEOUT:
        //                //        ChangeObject(o);
        //                //        break;
        //                //    default:
        //                //        HandleExceptionManaged(ex);
        //                //        break;
        //                //}
        //            }
        //            // }
        //         }

        #endregion

        #region IBaseBotEngine Members

        [Browsable(true)]
        [Category("Behavior")]
        [Description("When set to true, the bot will echo messages send into the chat room. It requires the world attribute echo chat setting to be turned of.")]
        public bool IsEchoChat
        {
            get
            {
                bool read_only;
                return aw.GetBool(AW.Attributes.WorldDisableChat); 
            } 

            set
            {
                int rc = aw.WorldAttributeSet((int)AW.Attributes.WorldDisableChat, "1");
                if (rc != 0)
                    throw new AwException(rc);
            }
        }

        [Browsable(false)]
        public LoginConfiguration LoginConfiguration
        {
            get { return _universeConnection; }
        }

        #endregion

        #region IConfigurable Members

        [Browsable(false)]
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
        [Browsable(false)]
        public IList<IConfigurable> Children { get; set; }

        #endregion

        #region ICanLog Members

        [Browsable(false)]
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

        [Browsable(true)]
        [Category("Behavior")]
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

            throw new AwException(instanceException.ErrorCode);
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

        public void ConsoleMessage(System.Drawing.Color color, bool isBold, bool isItalic, string message)
        {
            aw.SetInt(Attributes.ConsoleRed, color.R);
            aw.SetInt(Attributes.ConsoleGreen, color.G);
            aw.SetInt(Attributes.ConsoleBlue, color.B);
            aw.SetBool(Attributes.ConsoleBold, isBold);
            aw.SetBool(Attributes.ConsoleItalics, isItalic);
            aw.SetString(Attributes.ConsoleMessage, message);
            aw.ConsoleMessage(0);
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

        [Browsable(false)]
        public IServicesManager ServicesManager
        {
            get; internal set;
        }

        [Browsable(false)]
        public LocalBotPluginServicesManager LocalBotPluginServicesManager
        {
            get; internal set;
        }

        #region IIdentifiable Members

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Identification")]
        public string IdentifyableDisplayName
        {
            get; set;
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Identification")]
        public Guid IdentifyableId
        {
            get; set;
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Identification")]
        public string IdentifyableTechnicalName
        {
            get; set;
        }

        #endregion
    }
}