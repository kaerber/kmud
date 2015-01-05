using System.Collections.Generic;

namespace Kaerber.MUD.Telnet.Handlers {
    public class HandlerStateAutomaton {
        public HandlerStateAutomaton( Transition[,] table, State state ) {
            _table = table;
            _state = state;
        }

        public virtual void ReceiveCommand( Command command, TelnetHandler handler ) {
            var input = CommandToInput( CommandDirection.Received, command );
            ProcessInput( input, handler );
        }

        public virtual void SendCommand( Command command, TelnetHandler handler ) {
            var input = CommandToInput( CommandDirection.Sent, command );
            ProcessInput( input, handler );
        }

        public HandlerStateInput CommandToInput( CommandDirection direction, Command command ) {
            return ( HandlerStateInput )( direction + _commandIndex[command] );
        }

        private void ProcessInput( HandlerStateInput input, TelnetHandler handler ) {
            var transition = _table[( int )_state, ( int )input];
            if( transition != null )
                _state = transition.Execute( handler );
        }

        public State State { get { return _state; } }
        public bool Enabled { get { return _state == State.Yes; } }


        private readonly Transition[,] _table;
        private State _state;

        private static readonly Dictionary<Command,int> _commandIndex = new Dictionary<Command, int> {
            { Command.Do, 0 },
            { Command.Dont, 1 },
            { Command.Will, 2 },
            { Command.Wont, 3 }
        };

        public static HandlerStateAutomaton QMethodYesLocal( State state = State.No ) {
            return new HandlerStateAutomaton( HandlerStateTable.LocalQYesMethodTable, state );
        }

        public static HandlerStateAutomaton QMethodNoLocal( State state = State.No ) {
            return new HandlerStateAutomaton( HandlerStateTable.LocalQNoMethodTable, state );
        }

        public static HandlerStateAutomaton QMethodYesRemote( State state = State.No ) {
            return new HandlerStateAutomaton( HandlerStateTable.RemoteQYesMethodTable, state );
        }

        public static HandlerStateAutomaton QMethodNoRemote( State state = State.No ) {
            return new HandlerStateAutomaton( HandlerStateTable.RemoteQNoMethodTable, state );
        }
    }
}
