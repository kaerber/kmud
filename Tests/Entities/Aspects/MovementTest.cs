using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class MovementTest : BaseEntityTest {
        [Test]
        public void CloneTest() {
            var movement = AspectFactory.Movement();
            var clone = movement.Clone();
            Assert.IsNotNull( clone );
        }

        [Test]
        public void DeserializeReturnsSelf() {
            var movement = AspectFactory.Movement();
            var self = movement.Deserialize( null );
            Assert.IsNotNull( self );
        }

        [Test]
        public void WentFromRoomToRoom() {
            Event fromRoomEvent = null;
            Event toRoomEvent = null;


            var fromRoom = new Mock<Room>();
            fromRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( ev => { fromRoomEvent = ev; } );
            var toRoom = new Mock<Room>();
            toRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( ev => { toRoomEvent = ev; } );

            var mockExit = new Mock<Exit>();
            var mockEntrance = new Mock<Exit>();

            fromRoom.Setup( room => room.Exits[toRoom.Object] )
                .Returns( mockExit.Object );
            toRoom.Setup( room => room.Exits[fromRoom.Object] )
                .Returns( mockEntrance.Object );

            var movement = AspectFactory.Movement();
            movement.WentFromRoom( fromRoom.Object, toRoom.Object );

            Assert.IsNotNull( fromRoomEvent );
            Assert.AreEqual( "ch_went_from_room_to_room", fromRoomEvent.Name );
            Assert.IsNotNull( toRoomEvent );
            Assert.AreEqual( "ch_went_from_room_to_room", toRoomEvent.Name );
        }
    }
}
