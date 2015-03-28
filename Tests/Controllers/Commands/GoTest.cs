using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Controllers.Commands {
    [TestFixture]
    public class GoTest : BaseEntityTest {
        [Test]
        public void ExecuteTest() {
            var ch = new Character();
            var mockView = new Mock<ICharacterView>();
            var mockCommandManager = new Mock<IManager<ICommand>>();
            var pc = new CharacterController( ch, mockView.Object, mockCommandManager.Object, null );
            var command = new Go();
            command.Execute( pc, PlayerInput.Parse( "north" ) );
        }
    }
}
