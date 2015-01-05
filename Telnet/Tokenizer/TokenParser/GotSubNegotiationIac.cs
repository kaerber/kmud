using System;

namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotSubNegotiationIac : State {
        public GotSubNegotiationIac( Option option, ByteBuffer parameters ) {
            _option = option;
            _parameters = parameters;
        }

        public override State Parse( byte value, out Token token ) {
            if( value == Command.Se ) {
                token = TokenFactory.Instance.SubNegotiation( _option, _parameters );
                return StateFactory.Instance.Normal();
            }

            if( value == Command.Iac ) {
                token = null;
                _parameters.Append( value );
                return StateFactory.Instance.GotSubNegotiationParameters( _option, _parameters );
            }

            //TODO: Exceptional case
            throw new NotImplementedException();
        }

        private readonly Option _option;
        private readonly ByteBuffer _parameters;
    }
}
