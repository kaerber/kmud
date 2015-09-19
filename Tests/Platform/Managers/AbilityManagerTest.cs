using System.Linq;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Tests.Entities;
using Microsoft.Scripting.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class AbilityManagerTest : BaseEntityTest {
        [SetUp]
        public void Setup() {
            _manager = new AbilityManager( new PythonManager(), @"E:\Dev\Kaerber.MUD\Python\abilities" );
        }

        [Test]
        public void MovementTest() {
            var movement = _manager.Get( "movement" );
            var e = Event.Create( "test_event",
                                  EventReturnMethod.None,
                                  new EventArg( "message", "test" ) );
            movement.ReceiveEvent( e );

            Assert.AreEqual( "test", e["message"] );
        }

        [Test]
        public void ActionsOfMovementAbilityAreLoaded() {
            var movement = _manager.Get( "movement" );

            AssertSequenceEqual( new[] { "down", "east", "go", "north", "south", "up", "west" },
                                 movement.Actions.Keys.OrderBy( k => k ) );

        }

        private AbilityManager _manager;
    }
}
