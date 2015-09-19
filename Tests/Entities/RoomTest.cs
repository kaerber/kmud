using Kaerber.MUD.Entities.Aspects;

using Kaerber.MUD.Entities;
using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class RoomTest : BaseEntityTest {
        [Test]
        public void TickTest() {
            var room = new Room();
            var ch = new Character();
            room.Characters.Add( ch );
            room.Tick();
        }

        [Test]
        public void SelectCharactersTest() {
            var room = new Room();
            room.SelectCharacters( ch => true );
        }

        [Test]
        public void EventIsSentToAllCharactersInRoom() {
            var e = Event.Create( "test" );

            var mockCh1 = new Mock<Character>();
            mockCh1.Setup( ch => ch.ReceiveEvent( e ) );

            var mockCh2 = new Mock<Character>();
            mockCh2.Setup( ch => ch.ReceiveEvent( e ) );

            var room = new Room();
            room.Characters.Add( mockCh1.Object );
            room.Characters.Add( mockCh2.Object );

            room.ReceiveEvent( e );

            mockCh1.VerifyAll();
            mockCh2.VerifyAll();
        }

        [Test]
        public void AddCharacterTest() {
            var room = new Room { Id = "test-room", ShortDescr = "test room" };
            var ch = new Mock<Character>();

            var mockCharacterSet = new Mock<CharacterSet>();
            mockCharacterSet.Setup( set => set.Add( ch.Object ) );

            room.Characters = mockCharacterSet.Object;
            room.AddCharacter( ch.Object );

            mockCharacterSet.VerifyAll();
        }

        [Test]
        public void RemoveCharacterTest() {
            var room = new Room { Id = "test-room", ShortDescr = "test room" };
            var ch = new Mock<Character>();

            var mockCharacterSet = new Mock<CharacterSet>();
            mockCharacterSet.Setup( set => set.Remove( ch.Object ) );

            room.Characters = mockCharacterSet.Object;
            room.RemoveCharacter( ch.Object );

            mockCharacterSet.VerifyAll();
        }
    }
}
