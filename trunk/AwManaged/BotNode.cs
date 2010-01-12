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
using System.IO;
using System.Reflection;
using System.Threading;
using AW;
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;
using AwManaged.ExceptionHandling;
using AwManaged.Scene.Interfaces;

namespace AwManaged
{
    /// <summary>
    /// A bot node, is a clone of the masterbot. The tasks for a bot node is to send object add and remove commands.
    /// in order to speed up the universe.
    /// </summary>
    [Serializable]
    public class BotNode<TConnection, TModel> : INode, IRemoveObjects<TModel>, IAddObjects<TModel>, IChangedObjects<TModel>, ILogin<TConnection>
        where TModel : MarshalByRefObject, IModel<TModel>
        where TConnection : UniverseConnectionProperties
    {


        /// <summary>
        /// Reflected objeect instance of AW.Instance()
        /// </summary>
        internal object _aw;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.SetString(Attributes, Value)
        /// </summary>
        private MethodInfo _mSetString;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.SetString(Attributes, Value)
        /// </summary>
        private MethodInfo _mSetInt;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.GetBool(Attributes)
        /// </summary>
        private MethodInfo _mGetBool;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.ObjectChange()
        /// </summary>
        private MethodInfo _mObjectChange;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.ObjectChange()
        /// </summary>
        private MethodInfo _mObjectAdd;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.ObjectChange()
        /// </summary>
        private MethodInfo _mObjectDelete;
        /// <summary>
        /// Reflected method info voor dynamic invocation of aw.Login()
        /// </summary>
        private MethodInfo _mLogin;
        /// <summary>
        /// Reflected static method info for dynamic invocation of aw.Wait()
        /// </summary>
        private MethodInfo _mWait;
        /// <summary>
        /// Reflected static method info for dynamic invocation of aw.Enter(World)
        /// </summary>
        private MethodInfo _mEnter;
        /// <summary>
        /// Reflected static method info for dynamic invocation of aw.Enter(World)
        /// </summary>
        private MethodInfo _mStateChange;
        /// <summary>
        ///  Reflected static method info for dynamic invocation of aw.SetBool(Attribute, value)
        /// </summary>
        private MethodInfo _mSetBool;

        private IConnection<UniverseConnectionProperties> _loginConfiguration;

        private void refresh(object o)
        {
            lock (this)
            {
                Wait(0);
            }
        }

        private FileInfo _awsdk;
        private FileInfo _awnetsdk;


        public BotNode(UniverseConnectionProperties connection, int node)
        {
            Node = node;
            Connection = connection.Clone();
            Connection.LoginName += node;

            var awsdk = new FileInfo("awtemp.dll");
            var awnetsdk = new FileInfo("aw.core.dll");

            awsdk.Directory.CreateSubdirectory("node" + node);
            awsdk.CopyTo("node" + node + "\\aw.dll", true);
            awnetsdk.CopyTo("node" + node + "\\aw.core.dll", true);

            _awsdk = new FileInfo("node" + node + "\\aw.dll");
            _awnetsdk = new FileInfo("node" + node + "\\aw.core.dll");
        }

        public void Wait(int milliseconds)
        {
            _mWait.Invoke(_aw, new object[] {milliseconds});
        }

        public void AddObject(TModel model)
        {
            lock (this)
            {
                ObjectTransaction(model,ObjectTransactionType.Add);
            }
        }

        public void RemoveObject(TModel model)
        {
            lock (this)
            {
                ObjectTransaction(model, ObjectTransactionType.Remove);
            }
        }

        public void ChangeObject(TModel model)
        {
            lock (this)
            {
                ObjectTransaction(model, ObjectTransactionType.Change);
            }
        }

        #region IChangedObjects<TModel> Members

        public void ObjectTransaction(TModel o, ObjectTransactionType type)
        {
            lock (this)
            {
                SetInt(Attributes.ObjectId, o.Id);
                SetInt(Attributes.ObjectOldNumber, 0);
                SetInt(Attributes.ObjectOwner, o.Owner);
                SetInt(Attributes.ObjectType, (int)o.Type);
                SetInt(Attributes.ObjectX, (int)o.Position.x);
                SetInt(Attributes.ObjectY, (int)o.Position.y);
                SetInt(Attributes.ObjectZ, (int)o.Position.z);
                SetInt(Attributes.ObjectTilt, (int)o.Rotation.x);
                SetInt(Attributes.ObjectYaw, (int)o.Rotation.y);
                SetInt(Attributes.ObjectRoll, (int)o.Rotation.z);
                SetString(Attributes.ObjectDescription, o.Description);
                SetString(Attributes.ObjectAction, o.Action);
                SetString(Attributes.ObjectModel, o.ModelName);
                //if (o.Data != null) SetString(Attributes.ObjectData, o.Data);
                switch (type)
                {
                    case ObjectTransactionType.Add:
                        ObjectAdd();
                        break;
                    case ObjectTransactionType.Change:
                        ObjectChange();
                        break;
                    case ObjectTransactionType.Remove:
                        ObjectRemove();
                        break;
                }
            }
        }

        #endregion

        #region INode Members

        public int Node
        {
            get; private set;
        }

        #endregion

        #region ILogin<TConnection> Members

        public UniverseConnectionProperties Connection
        {
            get; private set;
        }


        private void ObjectChange()
        {
            var rc = (int)_mObjectChange.Invoke(_aw, null);
            if (rc != 0)
                throw new AwException(rc);
        }

        private void ObjectAdd()
        {
            var rc = (int)_mObjectAdd.Invoke(_aw, null);
            if (rc != 0)
                throw new AwException(rc);
        }

        private void ObjectRemove()
        {
            var rc = (int)_mObjectDelete.Invoke(_aw, null);
            if (rc != 0)
                throw new AwException(rc);
        }

        private bool GetBool(Attributes attribute)
        {
            return (bool)_mGetBool.Invoke(_aw, new object[] {(int) attribute});
        }

        private void SetString(Attributes attribute, string value)
        {
            _mSetString.Invoke(_aw, new object[] { (int)attribute, value });
        }

        private void SetInt(Attributes attribute, int value)
        {
            _mSetInt.Invoke(_aw, new object[] { (int)attribute, value });
        }

        private void SetBool(Attributes attribute, bool value)
        {
            _mSetBool.Invoke(_aw, new object[] { attribute, value });
        }

        private void Enter(string world)
        {
            _mEnter.Invoke(_aw, new object[] { world });
        }

        private void StateChange()
        {
            _mStateChange.Invoke(_aw, null);
        }

        public void Connect()
        {
            // load aw.core.dll in a seperate app.domain, so it uses a seperate copy of aw.dll C API.

            var path = _awnetsdk.Directory.FullName;

            var loSetup = new AppDomainSetup();
            loSetup.ApplicationBase = path;
            loSetup.ConfigurationFile = path + "\\app.config";
            loSetup.DisallowApplicationBaseProbing = false;
            loSetup.DisallowBindingRedirects = true;
            loSetup.ShadowCopyDirectories = path;
            loSetup.ShadowCopyFiles = path;
            loSetup.CachePath = _awnetsdk.Directory.FullName;
            loSetup.ApplicationName = "BotNode" + Node;
            loSetup.DynamicBase = path;
            loSetup.PrivateBinPath = path;
            loSetup.PrivateBinPathProbe = path;
            var loAppDomain = AppDomain.CreateDomain("BotNode" + Node, null,loSetup);
            loAppDomain.DomainManager.InitializeNewDomain(loSetup);
            loAppDomain.AppendPrivatePath(path);
            var asm = loAppDomain.Load("aw.core");
            //_aw = new Instance(Connection.Domain,Connection.Port);
            // invocations for the aw Instance.
            //var asm = Assembly.LoadFile(_awnetsdk.FullName);
            var t = asm.GetType(typeof(Instance).ToString());
            var constructor = asm.GetType(typeof(Instance).ToString()).GetConstructor(new[] { typeof(string), typeof(Int32) });
            var constructorParams = new object[2];
            constructorParams[0] = Connection.Domain;
            constructorParams[1] = Connection.Port;
            _aw = constructor.Invoke(constructorParams);
            _mSetString = t.GetMethod("SetString");
            _mGetBool = t.GetMethod("GetBool");
            _mSetBool = t.GetMethod("SetBool");
            _mSetInt = t.GetMethod("SetInt");
            _mObjectAdd = t.GetMethod("ObjectAdd");
            _mObjectChange = t.GetMethod("ObjectChange");
            _mObjectDelete = t.GetMethod("ObjectDelete");
            _mLogin = t.GetMethod("Login");
            _mEnter = t.GetMethod("Enter");
            _mStateChange = t.GetMethod("StateChange");
            // Invocations for the Wait Utility.
            t = asm.GetType(typeof(Utility).ToString());
            _mWait = t.GetMethod("Wait");
        }

        private Timer _timer;

        public void Login()
        {
            //AwHelpers.AwHelpers.Login(_aw, Connection,false);

            SetString(Attributes.LoginName, Connection.LoginName);
            SetString(Attributes.LoginPrivilegePassword, Connection.PrivilegePassword);
            SetInt(Attributes.LoginOwner, Connection.Owner);
            _mLogin.Invoke(_aw, null);
            // Have the bot enter the specified world under Care Taker Privileges (default/prefered)
            SetBool(Attributes.EnterGlobal, GetBool(Attributes.WorldCaretakerCapability));
            Enter(Connection.World);
            //Have the bot change state to 0n 0w 0a
            SetInt(Attributes.MyX, (int)Connection.Position.x); //X position of the bot (E/W)
            SetInt(Attributes.MyY, (int)Connection.Position.y); //Y position of the bot (height)
            SetInt(Attributes.MyZ, (int)Connection.Position.z); //Z position of the bot (N/S)
            SetInt(Attributes.MyPitch, (int)Connection.Position.y);
            SetInt(Attributes.MyYaw, (int)Connection.Position.z);
            StateChange();
            _timer = new Timer(refresh, null, 0, 13);
        }

        #endregion
    }

    public enum ObjectTransactionType
    {
        Add,
        Change,
        Remove
    }

    public interface IChangedObjects<TModel>
         where TModel : MarshalByRefObject, IModel<TModel>
    {
        void ChangeObject(TModel model);
    }

    public interface INode
    {
        int Node {get;}
    }

    public interface ILogin<TConnection>
          where TConnection : UniverseConnectionProperties
    {
        UniverseConnectionProperties Connection { get; }
        void Connect();
        void Login();
    }

    public interface IAddObjects<TModel>
        where TModel : MarshalByRefObject, IModel<TModel>
    {
        void AddObject(TModel model);
    }

    public interface IRemoveObjects<TModel>
        where TModel : MarshalByRefObject, IModel<TModel>
    {
        void RemoveObject(TModel model);
    }
}
