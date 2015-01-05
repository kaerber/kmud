using System;
using System.Text;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Telnet {
    [TestFixture]
    public class TelnetConnectionAcceptance {
        [SetUp]
        public void SetUp() {
            _mockSocket = new Mock<Socket>( null );
            _mockToken = new Mock<Token>();
            _mockTokenFactory = new Mock<TokenFactory>( MockBehavior.Strict
                );
            TokenFactory.Instance = _mockTokenFactory.Object;
            _mockTokenStream = new Mock<TokenStream>( null );
        }

        [TearDown]
        public void TearDown() {
            TokenFactory.Instance = new TokenFactory();
        }

        [Test]
        public void CreateAConnectionWithSocketProvided() {
            var tokenStream = new TokenStream( _mockSocket.Object );
            var connection = new TelnetConnection( tokenStream, TelnetHandlerSet.Default( tokenStream ) );

            Assert.IsNotNull( connection );
        }

        [Test]
        public void PollConnectionForDataAvailable() {
            TokenFactory.Instance = new TokenFactory();
            _mockSocket.Setup( socket => socket.Read() )
                .Returns( "test string\n".ToBytes() );
            var tokenStream = new TokenStream( _mockSocket.Object );
            var connection = new TelnetConnection( tokenStream, TelnetHandlerSet.Default( tokenStream ) );

            var data = string.Join( "", connection.ReadLines( Encoding.ASCII ) );

            Assert.AreEqual( "test string", data );
        }

        [Test]
        public void DisableLocalEchoForPlayer() {
            _mockTokenFactory.Setup( factory => factory.Command( Command.Do, Option.SuppressLocalEcho ) )
                .Returns( _mockToken.Object );
            _mockTokenStream.Setup( stream => stream.Write( _mockToken.Object ) );
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, 
                TelnetHandlerSet.Default( _mockTokenStream.Object ) );

            telnetConnection.SetRemote( Option.SuppressLocalEcho, true );

            _mockTokenStream.Verify( stream => stream.Write( _mockToken.Object ), Times.Once() );
        }

        [Test]
        public void OnReceivingCommandCallsCommandReceiveOfHandlerSet() {
            var mockHandlerSet = new Mock<TelnetHandlerSet>( _mockTokenStream.Object,
                                                             new Func<Option, TelnetHandler>( option => null ),
                                                             new TelnetHandler[0] );
            mockHandlerSet.Setup( set => set.ReceiveCommand( Option.SuppressLocalEcho, Command.Will ) );
            _mockTokenStream.Setup( stream => stream.Read() )
                .Returns( new[] { new TokenFactory().Command( Command.Will, Option.SuppressLocalEcho ) } );
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, mockHandlerSet.Object );

            var lines = telnetConnection.ReadLines( Encoding.ASCII );
            var text = string.Join( "", lines );

            mockHandlerSet.Verify( set => set.ReceiveCommand( Option.SuppressLocalEcho, Command.Will ), 
                Times.Once() );
        }

        private Mock<Socket> _mockSocket;
        private Mock<Token> _mockToken;
        private Mock<TokenFactory> _mockTokenFactory;
        private Mock<TokenStream> _mockTokenStream;
    }
}
