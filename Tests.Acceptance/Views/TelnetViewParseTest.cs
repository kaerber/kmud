using System.Linq;
using System.Text;

using Kaerber.MUD.Views;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Views {
    [TestFixture]
    public class TelnetViewParseTest : BaseAcceptanceTest {
        [SetUp]
        public void SetUp() {
            CreateTestEnvironment();
        }

        [Test]
        public void TelnetView_ParsesUserInputWithTelnetUserInputParser() {
            const string command = "get 2*2.'swo'";

            MockConnection.Setup( c => c.ReadLines( It.IsAny<Encoding>() ) )
                          .Returns( new[] { command } );
            MockParser.Setup( p => p.Parse( command, Model ) );

            var view = new TelnetCharacterView( MockConnection.Object,
                                                Model,
                                                MockRenderer.Object,
                                                MockParser.Object );

            var commands = view.Commands().ToList();

            MockParser.Verify( p => p.Parse( command, Model ), Times.Once() );
        }

        [Test]
        public void TestInvariant_CompiledParseItemReturnsListOfItems() {
            Assert.Fail();
        }
    }
}
