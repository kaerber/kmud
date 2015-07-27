using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class AbilityManagerTest {
        [Test]
        public void MovementTest() {
            var manager = new AbilityManager( new PythonManager() );
            var movement = manager.Movement();
            var e = Event.Create( "test_event",
                                  EventReturnMethod.None,
                                  new EventArg( "message", "test" ) );
            movement.ReceiveEvent( e );

            Assert.AreEqual( "Movement!", e["message"] );
        }
    }
}
