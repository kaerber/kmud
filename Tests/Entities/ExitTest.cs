using NUnit.Framework;

using Kaerber.MUD.Entities;
using Newtonsoft.Json;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class ExitTest : BaseEntityTest {
        [Test]
        public void SerializeTest() {
            var exit = new Exit( "west", "test_room" );
            var json = JsonConvert.SerializeObject( Exit.Serialize( exit ) );
            Assert.AreEqual( "{\"Name\":\"west\",\"To\":\"test_room\"}", json );
        }

        [Test]
        public void DeserializeTest() {
            var exit = Exit.Deserialize( JsonConvert.DeserializeObject( "{ 'Name': 'west', 'To': 'test_room' }" ) );
            Assert.AreEqual( "west", exit.Name );
            Assert.AreEqual( "test_room", exit.toRoom );
        }
    }
}
