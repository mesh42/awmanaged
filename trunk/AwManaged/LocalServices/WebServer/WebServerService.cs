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
using AwManaged.Core.Services;
using Cassini;

namespace AwManaged.LocalServices.WebServer
{
    public class WebServerService : BaseConnectedServiceDevice<WebServerConnection>
    {
        private Cassini.Server _server;

        public WebServerService(string connection) : base(connection)
        {
            foreach (var item in ConnectionStringHelper.GetNameValuePairs(Connection.ConnectionString, ProviderName))
            {
                switch (item.Name.ToLower())
                {
                    case "provider":
                        break; // handled by the ConnectionStringHelper
                    case "port":
                        try { Connection.Port = int.Parse(item.Value.Trim()); }
                        catch { ThrowIncorrectConnectionStringException(); }
                        break;
                    default:
                        ThrowIncorrectConnectionStringException();
                        break;
                }
            }
        }

        public override string ProviderName
        {
            get { return "webserver"; }
        }

        #region Service Members

        public override bool Start()
        {
            base.Start();

            var di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\www");
            if (!di.Exists)
                di.CreateSubdirectory("www");

            _server = new Server(Connection.Port,"/",di.FullName);
            
            _server.Start();
            return true;
        }

        public override bool Stop()
        {
            base.Stop();
            _server.Stop();
            return true;
        }

        public override string IdentifyableDisplayName
        {
            get { return "Web Service"; }
        }

        public override Guid IdentifyableId
        {
            get { return new Guid("{A75FDED5-3C54-4c37-A231-2F3EF3AA525F}"); }
        }

        public override void Dispose()
        {
            _server.Dispose();
        }

        #endregion


        internal override void EvaluateConnectionProperties()
        {
            throw new NotImplementedException();
        }
    }
}