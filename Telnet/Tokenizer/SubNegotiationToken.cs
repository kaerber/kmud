using System.Linq;

namespace Kaerber.MUD.Telnet.Tokenizer {
    public class SubNegotiationToken : Token {
        public SubNegotiationToken( Option option, ByteBuffer parameters ) {
            _option = option;
            _parameters = parameters;
        }

        public override byte[] Encode() {
            return new[] { Command.Iac, Command.Sb, ( byte )_option }
                .Concat( _parameters.EscapeIac() )
                .Concat( new byte[] { Command.Iac, Command.Se } )
                .ToArray();
        }

        private readonly Option _option;
        private readonly ByteBuffer _parameters;
    }
}
