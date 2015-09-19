using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Python.abilities {
    [TestFixture]
    public class AbilityTest {
        [Test]
        public void Scrap() {
            var manager = new AbilityManager( new PythonManager(), @"E:\Dev\Kaerber.MUD\Python\abilities" );
            var movement = manager.Movement();

            Event response;
            movement.EventSink = e => response = e;
            movement.ReceiveEvent( Event.Create( "test_event" ) );
        }
    }
}
