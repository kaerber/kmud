﻿using Kaerber.MUD.Tests.Entities;
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
            var pc = new CharacterController( ch, mockView.Object );
            var command = new Go();
            command.Execute( pc, PlayerInput.Parse( "north" ) );
        }
    }
}