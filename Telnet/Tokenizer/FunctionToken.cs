namespace Kaerber.MUD.Telnet.Tokenizer {
    public class FunctionToken : Token {

        public FunctionToken( Command command ) {
            _command = command;
        }

        public override byte[] Encode() {
            return new byte[] { Command.Iac, _command };
        }

        private readonly Command _command;
    }
}
