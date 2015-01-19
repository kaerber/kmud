using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Server.Managers;
using Kaerber.MUD.Telnet;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class Server {
        public Server() {
            _listeners = new List<TelnetListener>();
            _sessions = new List<ISession>();
            _container = UnityConfigurator.Configure();
        }

        public void Initialize() {
            World.Instance = World.Serializer.Deserialize<World>(
                File.ReadAllText( World.AssetsRootPath + "world.data" ) );
            World.Instance.LoadAreas();
            World.Instance.Initialize( _container );
            _container.RegisterInstance( World.Instance );

            var commandManager = new CommandManager();
            commandManager.Load();
            _container.RegisterInstance<IManager<ICommand>>( commandManager );

            var portNumber = int.Parse( ConfigurationManager.AppSettings.Get( "PortNumber" ) );
            var listener = new TelnetListener();
            AddListener( listener );
            listener.StartListening( portNumber );
        }

        public void AddListener( TelnetListener listener ) {
            _listeners.Add( listener );
            listener.ConnectionReceived += AcceptConnection;
        }

        public virtual void Pulse( long ticks ) {
            World.Instance.Pulse( ticks );
        }

        private void AcceptConnection( TelnetConnection telnetConnection ) {
            var container = TelnetSession.TelnetContainer( _container );
            container.RegisterInstance( telnetConnection );
            
            var session = container.Resolve<TelnetSession>();
            session.Start();
            _sessions.Add( session );
        }

        private long _round;
        private long _tick;

        private readonly UnityContainer _container;
        private readonly IList<TelnetListener> _listeners;
        private readonly IList<ISession> _sessions;
    }
}