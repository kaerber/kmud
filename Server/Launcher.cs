using System;
using System.Reflection;
using System.Threading;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Server {
    public class Launcher {
        static void Main() {
            log4net.Config.XmlConfigurator.Configure();

            InitializeML();

            var server = new Server();
            server.Initialize();
            
            while( true ) {
                server.Pulse( DateTime.Now.Ticks );
                Thread.Sleep( Clock.TimeStep );
            }
        }

        public static void InitializeML() {
            MLFunction.LoadAssemblies( Assembly.GetAssembly( typeof( CharacterController ) ) );
        }
    }
}
