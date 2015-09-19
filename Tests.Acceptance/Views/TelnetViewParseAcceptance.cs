using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Views {
    [TestFixture]
    public class TelnetViewParseAcceptance : BaseAcceptanceTest {
        [SetUp]
        public void SetUp() {
            ConfigureTelnetEnvironment();
            SetUpItems();
        }

        [Test]
        public void TelnetView_ParsesUserInputWithTelnetUserInputParser() {
            var mockParser = new Mock<ITelnetInputParser>();
            Container.RegisterInstance( mockParser.Object );
            CreateTestEnvironment();

            const string command = "get 2*2.'swo'";

            MockConnection.Setup( c => c.ReadLines( It.IsAny<Encoding>() ) )
                          .Returns( new[] { command } );
            mockParser.Setup( p => p.Parse( command, PlayerModel ) );

            var view = Container.Resolve<ICharacterView>();

            var commands = view.Commands().ToList();

            mockParser.Verify( p => p.Parse( command, PlayerModel ), Times.Once() );
        }

        [Test]
        [Ignore]
        public void CommandParamSword_ReturnsFirstSwordInRoom() {
            Test( With( () => ItemsInRoom( BronzeShield, SilverSword, MithrilSword ) ),
                  On( () => ParseCommand( "get sword" ) ),
                  YieldsCommand( "get" ).WithParameter( Set( SilverSword ) ) );
        }

        private void SetUpItems() {
            BronzeShield = CreateItem( "bronze shield", "bronze shield" );
            SilverSword = CreateItem( "silver sword", "silver sword" );
            MithrilSword = CreateItem( "mithril sword", "mithril sword" );
        }

        private IPlayerCommand ParseCommand( string command ) {
            FeedIntoConnection( command );
            //return PlayerView.
            return null;
        }

        private void FeedIntoConnection( params string[] commands ) {
            MockConnection.Setup( c => c.ReadLines( It.IsAny<Encoding>() ) )
                          .Returns( commands );
        }

        private static CommandAssertConstructor YieldsCommand( string name ) {
            return new CommandAssertConstructor( name );
        }

        private static IList<T> Set<T>( params T[] items ) {
            return items;
        }

        protected Item BronzeShield;
        protected Item SilverSword;
        protected Item MithrilSword;
    }
}
