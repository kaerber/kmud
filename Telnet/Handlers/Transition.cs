namespace Kaerber.MUD.Telnet.Handlers {
    public class Transition {
        public Transition( State nextState, Command output ) {
            _nextState = nextState;
            _output = output;
        }

        public State Execute( TelnetHandler handler ) {
            if( _output != null )
                handler.SendCommand( _output );
            return _nextState;
        }

        private readonly State _nextState;
        private readonly Command _output;
    }
}
