namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class StateFactory {
        public virtual State GotCommand( Command command ) {
            return new GotCommand( command );
        }

        public virtual State GotIac() {
            return new GotIac();
        }

        public virtual State GotSubNegotiationIac( Option option, ByteBuffer parameters ) {
            return new GotSubNegotiationIac( option, parameters );
        }

        public virtual State GotSubNegotiationOption( Option option ) {
            return new GotSubNegotiationOption( option );
        }

        public virtual State GotSubNegotiationParameters( Option option, ByteBuffer parameters ) {
            return new GotSubNegotiationParameters( option, parameters );
        }

        public virtual State GotSubNegotiationStart() {
            return new GotSubNegotiationStart();
        }

        public virtual State Normal() {
            return new Normal();
        }

        public static StateFactory Instance = new StateFactory();
    }
}
