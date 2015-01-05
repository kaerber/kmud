using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Controllers.Commands
{
    [TestFixture]
    public class SayTest : BaseEntityTest {
        [Test]
        public void ExecuteTest() {
            var mockView = new Mock<ICharacterView>();

            var room = new Room();
            var ch = new Character();

            ch.SetRoom( room );
            var command = new Say();

            command.Execute(
                new CharacterController( ch, mockView.Object ),
                PlayerInput.Parse( "say hey-hey-hey!" )
            );
        }
    }
}
