using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class RaceFactoryTest : BaseEntityTest {
        [Test]
        public void GetDefaultRace() {
            var race = RaceFactory.Default;

            Assert.IsNotNull( race );
            Assert.IsInstanceOf<Race>( race );
        }
    }
}
