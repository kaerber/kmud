using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class ExitSetTest : BaseEntityTest {
        [Test]
        public void IndexerTest() {
            var testRoom = new Room();
            var set = new ExitSet {
                new Exit { Name = "exit1", To = new Room() },
                new Exit { Name = "exit2", To = new Room() },
                new Exit { Name = "test", To = testRoom }
            };

            Assert.AreEqual( set["ex"].Name, "exit1" );
            Assert.AreEqual( set["exit2"].Name, "exit2" );
            Assert.IsNull( set["exit3"] );
            Assert.AreEqual( set["test"].To, testRoom );
            Assert.AreEqual( set[testRoom].Name, "test" );
        }
    }
}
