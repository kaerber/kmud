namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class GotCommand : State {
        private readonly Command _command;

        public GotCommand( Command command ) {
            _command = command;
        }
                
        public override State Parse( byte value, out Token token ) {
            token = TokenFactory.Instance.Command( _command, ( Option )value );
            return StateFactory.Instance.Normal();
        }
    }
}
