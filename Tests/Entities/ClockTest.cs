using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class ClockTest {
        [Test]
        public void Pulse_TicksAreConvertedToTime() {
            var clock = new Clock( 0 );

            clock.Pulse( 32*10000 );

            Assert.AreEqual( 32, clock.Time );
        }
    }
}
