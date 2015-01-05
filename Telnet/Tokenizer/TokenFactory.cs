namespace Kaerber.MUD.Telnet.Tokenizer {
    public class TokenFactory {
        public virtual Token Content( byte value ) {
            return new ContentToken( value );
        }

        public virtual Token Command( Command command, Option option ) {
            return new CommandToken( command, option );
        }

        public virtual Token Function( Command command ) {
            return new FunctionToken( command );
        }

        public virtual Token SubNegotiation( Option option, ByteBuffer parameters ) {
            return new SubNegotiationToken( option, parameters );
        }

        public static TokenFactory Instance = new TokenFactory();
    }
}
