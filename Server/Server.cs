using System.Collections.Generic;
using System.Configuration;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
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
            var world = _container.Resolve<World>();
            World.Instance = world;

            world.LoadAreas();

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

        private readonly UnityContainer _container;
        private readonly IList<TelnetListener> _listeners;
        private readonly IList<ISession> _sessions;
    }
}