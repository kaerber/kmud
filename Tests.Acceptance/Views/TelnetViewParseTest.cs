using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using Kaerber.MUD.Views;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Views {
    [TestFixture]
    public class TelnetViewParseTest : BaseAcceptanceTest {
        [SetUp]
        public void SetUp() {
            ConfigureTelnetContainer();
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
        public void GetSword_ReturnsFirstSwordInRoom() {
            MockConnection.Setup( c => c.ReadLines( It.IsAny<Encoding>() ) )
                          .Returns( new[] { "get sword" } );
        }

        [Test]
        public void TestInvariant_CompiledParseItemReturnsListOfItems() {
            Assert.Fail();
        }
    }
}
