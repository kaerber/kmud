using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class ExitTest : BaseEntityTest {
        [Test]
        public void SerializeTest()
        {
            var exit = new Exit( "west", "test_room" );
            var data = World.Serializer.Serialize( exit );
            Assert.AreEqual( "{\"Name\":\"west\",\"To\":\"test_room\"}", data );
        }

        [Test]
        public void DeserializeTest()
        {
            var exit = World.Serializer.Deserialize<Exit>( "{\"Name\":\"west\",\"To\":\"test_room\"}" );
            Assert.AreEqual( "west", exit.Name );
            Assert.AreEqual( "test_room", exit.toRoom );
        }
    }
}
