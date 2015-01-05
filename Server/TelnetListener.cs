using System;
using System.Net;
using System.Net.Sockets;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;
using Kaerber.MUD.Telnet.Tokenizer;

using SysSocket = System.Net.Sockets.Socket;
using System.Threading;
using System.Threading.Tasks;

using MudSocket = Kaerber.MUD.Telnet.Socket;

namespace Kaerber.MUD.Server {
    public class TelnetListener {
        public void StartListening( int port, int backlogSize = 5 ) {
            if( _server != null )
                throw new InvalidOperationException( "Listener already started" );

            _server = new SysSocket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            _server.Bind( new IPEndPoint( IPAddress.Any, port ) );

            _cancellation = new CancellationTokenSource();
            var token = _cancellation.Token;

            _task = new Task( () => Listen( backlogSize, token ), token, TaskCreationOptions.LongRunning );
            _task.Start();
        }

        public void StopListening() {
            if( _task == null )
                throw new InvalidOperationException( "Listening is not yet started." );

            _cancellation.Cancel();

            _server.Shutdown( SocketShutdown.Send );

            var buffer = new byte[100];
            try {
                while( _server.Receive( buffer ) != 0 );
            }
            finally {
                _server.Close();
                _server = null;
            }
        }


        public void Accept( MudSocket socket ) {
            var tokenStream = new TokenStream( socket );
            var connection = new TelnetConnection( tokenStream, TelnetHandlerSet.Default( tokenStream ) );
            ConnectionReceived( connection );
        }


        private void Listen( int backlogSize, CancellationToken token ) {
            _server.Listen( backlogSize );

            while( !token.IsCancellationRequested ) {
                var socket = new MudSocket( _server.Accept() );
                Accept( socket );
            }

            token.ThrowIfCancellationRequested();
        }

        private SysSocket _server;

        private Task _task;
        private CancellationTokenSource _cancellation;

        public event Action<TelnetConnection> ConnectionReceived = delegate {};
    }
}
