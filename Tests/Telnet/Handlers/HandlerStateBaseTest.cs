using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Telnet.Handlers
{
    public class HandlerStateBaseTest {
        protected void Prepare( State state, bool local ) {
            Handler = new Mock<TelnetHandler>( Option.SuppressLocalEcho, null, null );
            Handler.Setup( handler => handler.SendCommand( It.IsAny<Command>() ) );
            Handler.Setup( handler => handler.SendCommand( Command.Do ) );
            Handler.Setup( handler => handler.SendCommand( Command.Dont ) );

            Automaton = new HandlerStateAutomaton( local
                                                       ? HandlerStateTable.LocalQYesMethodTable 
                                                       : HandlerStateTable.RemoteQYesMethodTable,
                                                   state );
        }

        protected void OnReceiving( Command command ) {
            Automaton.ReceiveCommand( command, Handler.Object );
        }

        protected void OnSending( Command command ) {
            Automaton.SendCommand( command, Handler.Object );
        }

        protected void ChangeTo( State state ) {
            Assert.AreEqual( state, Automaton.State );
        }

        protected void StayIn( State state ) {
            Assert.AreEqual( state, Automaton.State );
        }

        protected void AndSend( Command command ) {
            if( command == null ) {
                Handler.Verify( handler => handler.SendCommand( It.IsAny<Command>() ), Times.Never() );
                return;
            }

            Handler.Verify( handler => handler.SendCommand( command ), Times.Once() );

        }

        protected Mock<TelnetHandler> Handler;
        protected HandlerStateAutomaton Automaton;

        protected readonly Command Will = Command.Will;
        protected readonly Command Wont = Command.Wont;
        protected readonly Command Do = Command.Do;
        protected readonly Command Dont = Command.Dont;
        protected readonly Command Nothing = null;
    }
}
