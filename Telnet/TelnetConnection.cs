using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kaerber.MUD.Telnet.Handlers;
using Kaerber.MUD.Telnet.Tokenizer;

namespace Kaerber.MUD.Telnet {
    public class TelnetConnection {
        public TelnetConnection( TokenStream stream, TelnetHandlerSet set ) {
            _stream = stream;
            _set = set;
        }

        public virtual IEnumerable<string> ReadLines( Encoding encoding ) {
            var builder = new StringBuilder();
            foreach( var token in _stream.Read() ) {
                if( token.Handle( this ) )
                    continue;
                builder.Append( token.Content( encoding ) );
                var line = builder.GetLine();
                if( line != null )
                    yield return line;
            }
        }

        public virtual void Write( string text ) {
            foreach( var token in text.Select( c => TokenFactory.Instance.Content( c.ToByte() ) ) )
                _stream.Write( token );
        }

        public virtual void SetRemote( Option option, bool value ) {
            _set.SetRemote( option, value, _stream );
        }

        public virtual void Set( Option option, bool value ) {
            _set.Set( option, value, _stream );
        }

        public virtual void ReceiveCommand( Option option, Command command ) {
            _set.ReceiveCommand( option, command );
        }

        public void Close() {
            _stream.Close();
        }

        public void SetOutputVisibility( bool visible ) {
            SetRemote( Option.SuppressLocalEcho, !visible );
            var echo = _set.GetHandler( Option.Echo ) as EchoHandler;
            if( echo != null )
                echo.Echo = visible;
        }

        private readonly TokenStream _stream;
        private readonly TelnetHandlerSet _set;
    }
}
