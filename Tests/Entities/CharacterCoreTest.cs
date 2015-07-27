using System;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class CharacterCoreTest {
        [Test]
        public void Scrap() {
            var manager = new AbilityManager( new PythonManager() );
            var movement = manager.Movement();

            dynamic core = new CharacterCore();
            core.Movement = movement;
            Event response;
            core.EventSink = ( Action<Event> )( e => response = e );
            core.ReceiveEvent( Event.Create( "test_event" ) );
        }
    }
}
