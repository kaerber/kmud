namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotSubNegotiationOption : State {
        private readonly Option _option;

        public GotSubNegotiationOption( Option option ) {
            _option = option;
        }

        public override State Parse( byte value, out Token token ) {
            if( value == Command.Iac ) {
                token = null;
                return StateFactory.Instance.GotSubNegotiationIac( _option, new ByteBuffer() );
            }

            token = null;
            return StateFactory.Instance.GotSubNegotiationParameters( _option, new ByteBuffer( value ) );
        }
    }
}
