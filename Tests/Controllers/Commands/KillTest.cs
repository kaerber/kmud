using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;

using Moq;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Controllers.Commands {
    [TestFixture]
    public class KillTest {
        public class CharacterMock : Character {
            public CharacterMock() : base( new CharacterCore() ) {}

            private int _killCallCount;

            public override void Kill( Character vch ) {
                _killCallCount++;
            }

            public void Verify( int killCallCount ) {
                Assert.AreEqual( killCallCount, _killCallCount );
            }
        }

        public class ShimKillCommand : Kill {
            private readonly Character _vch;

            public ShimKillCommand( Character self, Character vch ) {
                Self = self;
                _vch = vch;
            }

            public override void Execute( ICharacterController pc, PlayerInput input ) {
                KillChar( _vch );
            }
        }

        [Test]
        public void Create() {
            var command = new Kill();
        }

        [Test]
        public void PayloadTest() {
            var controller = new Mock<ICharacterController>();
            var ch = new CharacterMock();
            var command = new ShimKillCommand( ch, new CharacterMock() );

            command.Execute( controller.Object, PlayerInput.Parse( "kill enemy" ) );

            ch.Verify( 1 );
        }
    }
}
