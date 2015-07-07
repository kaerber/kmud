using System.Collections.Generic;
using System.Linq;

using SysSocket = System.Net.Sockets.Socket;

namespace Kaerber.MUD.Telnet {
    public class Socket {
        public Socket( SysSocket socket ) {
            _socket = socket;
            Connected = true;
        }

        public bool Connected { get; private set; }

        public virtual IEnumerable<byte> Read() {
            _receivedIndex = -1;
            while( Connected )
                yield return GetByte();
        }

        public virtual void Write( IEnumerable<byte> content ) {
            _socket.Send( content.ToArray() );
        }

        public void Close() {
            _socket.Close();
            Connected = false;
        }

        private byte GetByte() {
            if( _receivedIndex == -1 ) {
                GetBuffer();
                _receivedIndex = 0;
            }

            var b = _received[_receivedIndex];
            _receivedIndex++;
            if( _receivedIndex >= _received.Length )
                _receivedIndex = -1;

            return b;
        }

        private void GetBuffer() {
            var size = _socket.Receive( _buffer );
            _received = _buffer.Take( size ).ToArray();

        }

        private readonly SysSocket _socket;
        private readonly byte[] _buffer = new byte[1024];

        private byte[] _received;
        private int _receivedIndex;
    }
}
