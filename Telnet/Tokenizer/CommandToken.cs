using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Telnet.Tokenizer {
    public class CommandToken : Token {
        public CommandToken( Command verb, Option option ) {
            Contract.Requires( verb.Negotiation );
            _verb = verb;
            _option = option;
        }

        public override byte[] Encode() {
            return new[] { Command.Iac, _verb, ( byte )_option };
        }

        public override bool Handle( TelnetConnection connection ) {
            Contract.Requires( connection != null );
            connection.ReceiveCommand( _option, _verb );
            return true;
        }

        private readonly Command _verb;
        private readonly Option _option;
    }
}
