using Kaerber.MUD.Telnet.Tokenizer;

namespace Kaerber.MUD.Telnet.Handlers {
    public class TelnetHandler {
        public TelnetHandler( Option option, 
                              HandlerStateAutomaton localState = null, 
                              HandlerStateAutomaton remoteState = null ) {
            Option = option;
            LocalState = localState;
            RemoteState = remoteState;
        }

        public virtual Option Option { get; private set; }

        public virtual TokenStream TokenStream { protected get; set; }

        public bool Enabled { get { return LocalState.Enabled; } }

        public virtual void SetRemote( bool state ) {
            var command = Command.RemoteRequest( state );
            LocalState.SendCommand( command, this );
            RemoteState.SendCommand( command, this );
        }

        public virtual void Set( bool state ) {
            var command = Command.LocalRequest( state );
            LocalState.SendCommand( command, this );
            RemoteState.SendCommand( command, this );
        }

        public virtual void ReceiveCommand( Command command ) {
            LocalState.ReceiveCommand( command, this );
            RemoteState.ReceiveCommand( command, this );
        }


        public virtual void SendCommand( Command command ) {
            TokenStream.Write( TokenFactory.Instance.Command( command, Option ) );
        }

        protected HandlerStateAutomaton LocalState;
        protected HandlerStateAutomaton RemoteState;

        public static TelnetHandler QMethodAgreeingHandler( Option option ) {
            return new TelnetHandler( option,
                                      HandlerStateAutomaton.QMethodYesLocal(),  
                                      HandlerStateAutomaton.QMethodYesRemote() );
        }

        public static TelnetHandler QMethodDisagreeingHandler( Option option ) {
            return new TelnetHandler( option,
                                      HandlerStateAutomaton.QMethodNoLocal(),
                                      HandlerStateAutomaton.QMethodNoRemote() );
        }
    }
}
