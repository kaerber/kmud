namespace Kaerber.MUD.Telnet.Handlers {
    public class HandlerState {

        public virtual HandlerState ReceiveCommand( Command command, TelnetHandler handler ) {
            return this;
        }

        public virtual HandlerState SendCommand( Command command, TelnetHandler handler ) {
            return this;
        }
    }
}
