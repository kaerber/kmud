using System;
using System.Text;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Handlers;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Telnet {
    [TestFixture]
    public class TelnetConnectionTest {
        [SetUp]
        public void SetUp() {
            _mockToken = new Mock<Token>();
            _mockTokenFactory = new Mock<TokenFactory>();
            _mockTokenFactory.Setup( factory => factory.Content( 'T'.ToByte() ) )
                .Returns( _mockToken.Object );
            TokenFactory.Instance = _mockTokenFactory.Object;
            _mockTokenStream = new Mock<TokenStream>( null );
        }

        [TearDown]
        public void TearDown() {
            TokenFactory.Instance = new TokenFactory();
        }

        [Test]
        public void WriteConvertsStringToTokens() {
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, 
                TelnetHandlerSet.Default( _mockTokenStream.Object ) );

            telnetConnection.Write( "T" );

            _mockTokenFactory.Verify( factory => factory.Content( 'T'.ToByte() ), Times.Once() );
        }

        [Test]
        public void WriteCallsTokenStreamWriteForAConvertedToken() {
            _mockTokenStream.Setup( stream => stream.Write( _mockToken.Object ) );
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, 
                TelnetHandlerSet.Default( _mockTokenStream.Object ) );

            telnetConnection.Write( "T" );

            _mockTokenStream.Verify( stream => stream.Write( _mockToken.Object ), Times.Once() );
        }

        [Test]
        public void SetRemoteCallsHandlerSet() {
            var mockHandlerSet = new Mock<TelnetHandlerSet>( _mockTokenStream.Object,
                                                             new Func<Option, TelnetHandler>( option => null ),
                                                             new TelnetHandler[0] );
            mockHandlerSet.Setup( set => set.SetRemote( Option.SuppressLocalEcho, 
                                                        true, 
                                                        _mockTokenStream.Object ) );
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, mockHandlerSet.Object );

            telnetConnection.SetRemote( Option.SuppressLocalEcho, true );

            mockHandlerSet.Verify( set => set.SetRemote( Option.SuppressLocalEcho, 
                                                         true, 
                                                         _mockTokenStream.Object ) );
        }

        [Test]
        public void OnReceivingCommandCallsCommandReceiveOfHandlerSet() {
            var mockHandlerSet = new Mock<TelnetHandlerSet>( _mockTokenStream.Object,
                                                             new Func<Option, TelnetHandler>( option => null ),
                                                             new TelnetHandler[0] );
            var mockToken = new Mock<Token>();
            mockToken.Setup( token => token.Handle( It.IsAny<TelnetConnection>() ) )
                .Returns( true );
            _mockTokenStream.Setup( stream => stream.Read() )
                .Returns( new[] { mockToken.Object } );
            var telnetConnection = new TelnetConnection( _mockTokenStream.Object, mockHandlerSet.Object );

            var lines = string.Join( "", telnetConnection.ReadLines( Encoding.ASCII ) );

            mockToken.Verify( token => token.Handle( It.IsAny<TelnetConnection>() ), Times.Once() );
        }

        private Mock<Token> _mockToken;
        private Mock<TokenFactory> _mockTokenFactory;
        private Mock<TokenStream> _mockTokenStream;
    }
}
