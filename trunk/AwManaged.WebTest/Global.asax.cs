using SharedMemory;using System;
using AwManaged.Interfaces;
using AwManaged.LocalServices;
using AwManaged.RemoteServices;
using AwManaged.Scene;
using AwManaged.Storage;
using Cassini;

namespace AwManaged.WebTest
{
    public class Global : System.Web.HttpApplication
    {
        public static IBotEngine<Avatar, Model, Camera, Zone, Mover, HudBase<Avatar>, Scene.Particle,
Scene.ParticleFlags, Db4OConnection,
LocalBotPluginServicesManager> Bot = (IBotEngine<Avatar, Model, Camera, Zone, Mover, HudBase<Avatar>, Scene.Particle,
         Scene.ParticleFlags, Db4OConnection,
         LocalBotPluginServicesManager>)CrossAppDomainSingletonT<CrossAppDomainSingleton>.Instance.Shared;

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}