using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Tests.Entities;
using Newtonsoft.Json;
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

        [Test]
        public void ParametersOfGoActionAreTransferredCorrectlyFromPython() {
            var movement = _manager.Get( "movement" );
            var action = movement.Actions["go"];
            AssertSequenceEqual( new[] { "room-exit" }, action.Parameters() );
        }

        [Test]
        public void QueryActionsOfGoAbilityReturnsCorrectActions() {
            var movement = _manager.Get( "movement" );
            var e = Event.Create( "query_actions",
                                  EventReturnMethod.None,
                                  new EventArg( "actions", new Dictionary<string, IAction>() ) );
            movement.ReceiveEvent( e );
            Func<string, string> keySelector = k => k;
            AssertSequenceEqual( new[] { "down", "east", "go", "north", "south", "up", "west" },
                                 Enumerable.OrderBy( e["actions"].Keys, keySelector ) );
        }

        [Test]
        public void SetStatesCopiesStateToAbility() {
            dynamic movement = _manager.Get( "movement" );
            dynamic data = JsonConvert.DeserializeObject(
                File.ReadAllText( @"E:\Dev\Kaerber.MUD\Assets\players\Luch1\characters\kaerber\abilities\movement.json" ) );
            movement.SetState( data );

            Assert.AreEqual( data.location.Value, movement.state.location );
        }

        [Test]
        public void ListReturnsListOfAbilities() {
            var list = _manager.List( @"E:\Dev\Kaerber.MUD\Assets\players\Luch1\characters\kaerber" );

            AssertSequenceEqual( new[] { "movement" }, list );
        }

        [Test]
        public void LoadReturnsAbilityWithLoadedState() {
            dynamic ability = _manager.Load( @"E:\Dev\Kaerber.MUD\Assets\players\Luch1\characters\kaerber",
                                              "movement" );

            Assert.AreEqual( "heaven_02", ability.state.location );
        }


        private AbilityManager _manager;
    }
}
