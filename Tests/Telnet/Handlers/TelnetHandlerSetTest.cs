using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class TelnetHandlerSetTest {
        [SetUp]
        public void SetUp() {
            _handler = new Mock<TelnetHandler>( Option.SuppressLocalEcho, null, null );
            _handler.Setup( h => h.Option ).Returns( Option.SuppressLocalEcho );

            _defaultHandler = new Mock<TelnetHandler>( Option.Status, null, null );
            _defaultHandler.Setup( h => h.Option ).Returns( Option.Status );
            
            _tokenStream = new Mock<TokenStream>( null );
            _tokenStream.Setup( stream => stream.Write( It.IsAny<Token>() ) );

            _handlerSet = new TelnetHandlerSet( _tokenStream.Object, option => _defaultHandler.Object, _handler.Object );
        }

        [Test]
        public void SetRemoteForOptionCallsOptionHandler() {
            _handler.Setup( handler => handler.SetRemote( true ) );

            _handlerSet.SetRemote( Option.SuppressLocalEcho, true, null );

            _handler.Verify( handler => handler.SetRemote( true ), Times.Once() );
        }

        [Test]
        public void CommandReceivedCallsOptionHandler() {
            _handler.Setup( handler => handler.ReceiveCommand( Command.Will ) );

            _handlerSet.ReceiveCommand( Option.SuppressLocalEcho, Command.Will );

            _handler.Verify( handler => handler.ReceiveCommand( Command.Will ), Times.Once() );
        }

        [Test]
        public void OnReceivingUnsupportedCommand_UseDefaultHandler() {
            _handler.Setup( h => h.ReceiveCommand( It.IsAny<Command>() ) );
            _defaultHandler.Setup( h => h.ReceiveCommand( It.IsAny<Command>() ) );

            _handlerSet.ReceiveCommand( Option.Status, Command.Will );

            _handler.Verify( h => h.ReceiveCommand( It.IsAny<Command>() ), Times.Never() );
            _defaultHandler.Verify( h => h.ReceiveCommand( It.IsAny<Command>() ), Times.Once() );
        }

        [Test]
        public void OnSetRemoteForUnsupportedCommand_DoNothing() {
            _handler.Setup( h => h.SetRemote( It.IsAny<bool>() ) );

            _handlerSet.SetRemote( Option.Status, true, null );

            _handler.Verify( h => h.SetRemote( It.IsAny<bool>() ), Times.Never() );
        }

        private Mock<TelnetHandler> _handler;
        private Mock<TelnetHandler> _defaultHandler;

        private Mock<TokenStream> _tokenStream;
        private TelnetHandlerSet _handlerSet;
    }
}
