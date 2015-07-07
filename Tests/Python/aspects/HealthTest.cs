using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Python.aspects {
    [TestFixture]
    public class HealthTest {
        [Test]
        public void GainHealth_IncreasesHealthByAmount() {
            var aspect = AspectFactory.Health();
            aspect.Wounds = 100;

            aspect.GainHealth( 60 );

            Assert.AreEqual( 40, aspect.Wounds );
        }
    }
}
