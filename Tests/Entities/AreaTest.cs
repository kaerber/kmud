using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class AreaTest : BaseEntityTest {
        [Test]
        public void GetRoomTest() {
            var area = new Area();
            var room = new Room { Id = "test_room" };
            area.Rooms.Add( room );

            Assert.AreEqual( room, area.GetRoom( "test_room" ) );
            Assert.IsNull( area.GetRoom( "room_not_exists" ) );
        }
    }
}
