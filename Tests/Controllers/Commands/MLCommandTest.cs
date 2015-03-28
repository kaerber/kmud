using System;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

using Kaerber.MUD.Tests.Entities;
using Moq;
using NUnit.Framework;


namespace Kaerber.MUD.Tests.Controllers.Commands
{
    [TestFixture]
    public class MLCommandTest : BaseEntityTest {
        [Test]
        public void ExecuteTest() {
            var mockCharacter = new Mock<Character>();
            var mockView = new Mock<ICharacterView>();
            var mockManager = new Mock<IManager<ICommand>>();
            var pc = new CharacterController( mockCharacter.Object, 
                                              mockView.Object, 
                                              mockManager.Object,
                                              null );
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
