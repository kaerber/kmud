using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaerber.MUD.Telnet {
    public class ByteBuffer {
        public ByteBuffer() {
            _buffer = new List<byte>();
        }

        public ByteBuffer( byte value ) {
            _buffer = new List<byte> { value };
        }

        public ByteBuffer( IEnumerable<byte> bytes ) {
            _buffer = new List<byte>( bytes );
        }

        public void Append( byte value ) {
            _buffer.Add( value );
        }

        public byte[] EscapeIac() {
            return _buffer.SelectMany( b => b != 0xff ? new[] { b } : new[] { b, b } )
                .ToArray();
        }

        public string ToString( Encoding encoding ) {
            return new string( encoding.GetChars( _buffer.ToArray() ) );
        }

        public override bool Equals( object other ) {
            var o = other as ByteBuffer;
            if( ( object )o == null ) return false;
            return this == o;
        }

        public override int GetHashCode() {
            unchecked {
                var hash = 17;
                _buffer.ForEach( item => hash = hash*23 + item.GetHashCode() );

                return hash;
            }
        }

        public override string ToString() {
            return string.Format( "[{0}]", 
                string.Join( ", ", _buffer.Select( b => string.Format( "{0:x}", b ) ) ) );
        }

        public static bool operator ==( ByteBuffer self, ByteBuffer other ) {
            if( ( object )self == null && ( object )other == null ) return true;
            if( ( object )self == null ) return false;
            if( ( object )other == null ) return false;

            return self._buffer.SequenceEqual( other._buffer );
        }

        public static bool operator !=( ByteBuffer self, ByteBuffer other ) {
            return !( self == other );
        }

        private readonly List<byte> _buffer;
    }
}
