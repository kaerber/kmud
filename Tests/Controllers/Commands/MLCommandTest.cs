using System;

using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Controllers.Commands
{
    [TestFixture]
    public class MLCommandTest : BaseEntityTest {
        [Test]
        public void ExecuteTest() {
            var mockView = new Mock<ICharacterView>();
            var pc = new CharacterController( new Character(), mockView.Object );
            var command = new MLCommand( "divideByZero", "10/0" );
            Assert.Throws<DivideByZeroException>( 
                () => command.Execute( pc, PlayerInput.Parse( "test" ) )
            );
        }

        [Test]
        public void ToStringName_ReturnsCommandName() {
            var command = new MLCommand( "command", "pass" );

            var name = command.ToString( "name", null );

            Assert.AreEqual( "command", name );
        }

        public void StringFormatName_ReturnsCommandName() {
            var command = new MLCommand( "command", "pass" );

            var name = string.Format( "{0:name}", command );

            Assert.AreEqual( "command", name );
        }
    }
}
