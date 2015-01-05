namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotSubNegotiationParameters : State {
        public GotSubNegotiationParameters( Option option, ByteBuffer parameters ) {
            _option = option;
            _parameters = parameters;
        }

        public override State Parse( byte value, out Token token ) {
            if( value == Command.Iac ) {
                token = null;
                return StateFactory.Instance.GotSubNegotiationIac( _option, _parameters );
            }

            token = null;
            _parameters.Append( value );
            return StateFactory.Instance.GotSubNegotiationParameters( _option, _parameters );
        }

        private readonly Option _option;
        private readonly ByteBuffer _parameters;
    }
}
