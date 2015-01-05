using System;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Controllers.Commands {
    [TestFixture]
    public class KillTest {
        public class ControllerMock : ICharacterController {
            public Character Model {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ICharacterView View {
                get { throw new NotImplementedException(); }
            }

            public User User {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ISession Session {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public void InputReceived( string line ) {
                throw new NotImplementedException();
            }

            public void SetDefaultCommandSet() {
                throw new NotImplementedException();
            }

            public void SetCommandSet( string name ) {
                throw new NotImplementedException();
            }

            public void Quit() {
                throw new NotImplementedException();
            }

            public IController Start() {
                throw new NotImplementedException();
            }

            public IController Run() {
                throw new NotImplementedException();
            }

            public IController Stop() {
                throw new NotImplementedException();
            }
        }

        public class CharacterMock : Character {
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
            var controller = new ControllerMock();
            var ch = new CharacterMock();
            var command = new ShimKillCommand( ch, new CharacterMock() );

            command.Execute( controller, PlayerInput.Parse( "kill enemy" ) );

            ch.Verify( 1 );
        }
    }
}
