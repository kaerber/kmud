namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotIac : State {
        public override State Parse( byte value, out Token token ) {
            if( value == Command.Iac ) {
                token = TokenFactory.Instance.Content( value );
                return StateFactory.Instance.Normal();
            }

            var command = Command.Parse( value );
            if( command.Negotiation ) {
                token = null;
                return StateFactory.Instance.GotCommand( command );
            }

            if( command == Command.Sb ) {
                token = null;
                return StateFactory.Instance.GotSubNegotiationStart();
            }

            token = TokenFactory.Instance.Function( command );
            return StateFactory.Instance.Normal();
        }
    }
}
