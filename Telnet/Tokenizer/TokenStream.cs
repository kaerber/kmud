using System;
using System.Collections.Generic;

using Kaerber.MUD.Telnet.Tokenizer.TokenParser;

namespace Kaerber.MUD.Telnet.Tokenizer {
    public class TokenStream {
        public TokenStream( Socket socket ) {
            _socket = socket;
        }

        public event Action<Token> TokenReceived;
        public virtual void Write( Token token ) {
            _socket.Write( token.Encode() );
        }

        public virtual IEnumerable<Token> Read() {
            var state = StateFactory.Instance.Normal();
            foreach( var datum in _socket.Read() ) {
                Token token;
                state = state.Parse( datum, out token );
                if( token == null )
                    continue;

                if( TokenReceived != null )
                    TokenReceived( token );
                yield return token;
                
            }
        }

        public virtual void Close() {
            _socket.Close();
        }

        private readonly Socket _socket;
    }
}
