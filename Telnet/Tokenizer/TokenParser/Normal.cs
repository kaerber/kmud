namespace Kaerber.MUD.Telnet.Tokenizer.TokenParser {
    public class Normal : State {
        public override State Parse( byte value, out Token token ) {
            token = null;

            if( value == Command.Iac )
                return StateFactory.Instance.GotIac();
           
            token = TokenFactory.Instance.Content( value );
            return this;
        }
    }
}
