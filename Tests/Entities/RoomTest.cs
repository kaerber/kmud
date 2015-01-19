using Kaerber.MUD.Entities.Aspects;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Server;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class RoomTest : BaseEntityTest {
        [Test]
        public void CastAffectToRoomTest() {
            World.Instance = new World();
            World.Instance.Initialize( UnityConfigurator.Configure() );

            var room = new Room();
            room.Affects.Add( new Affect( new AffectInfo {
                Name = "horde_room",
                Target = AffectTarget.Room
            } )
            { Duration = Clock.TimeHour } );

            Assert.AreEqual( 1, room.Affects.Count );
            Assert.AreEqual( "horde_room", room.Affects[0].Name );

            Assert.Throws<EntityException>( () => room.Affects.Add(
                    new Affect( new AffectInfo {
                            Name = "horde_mob",
                            Target = AffectTarget.Character
                        }
                    )
                    { Duration = -1 }
                )
            );
            Assert.AreEqual( 1, room.Affects.Count );
        }

        [Test]
        public void TickTest() {
            var room = new Room();
            var ch = new Character();
            room.Characters.Add( ch );
            room.Tick();
        }

        [Test]
        public void AreaTest() {
            World.Instance = new World();
            World.Instance.Areas.Add( new Area() );
            var room = new Room { Id = "test_room" };
            World.Instance.Areas[ 0 ].Rooms.Add( room );

            Assert.AreEqual( World.Instance.Areas[ 0 ], room.Area );
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
