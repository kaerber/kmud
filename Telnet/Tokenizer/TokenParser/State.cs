namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public abstract class State {
        public abstract State Parse( byte value, out Token token );
    }
}
