namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotSubNegotiationStart : State {
        public override State Parse( byte value, out Token token ) {
            token = null;
            return new GotSubNegotiationOption( ( Option )value );
        }
    }
}
