using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class TelnetHandlerTest {
        [Test]
        public void ReceiveCommandForwardsCommandToState() {
            var mockLocal = new Mock<HandlerStateAutomaton>( null, State.No );
            var mockRemote = new Mock<HandlerStateAutomaton>( null, State.No );
            var handler = new TelnetHandler( Option.SuppressLocalEcho, mockLocal.Object, mockRemote.Object );
            mockRemote.Setup( automaton => automaton.ReceiveCommand( Command.Will, handler ) );
            mockLocal.Setup( automaton => automaton.ReceiveCommand( Command.Will, handler ) );

            handler.ReceiveCommand( Command.Will );

            mockRemote.Verify( automaton => automaton.ReceiveCommand( Command.Will, handler ), Times.Once() );
        }
    }
}
