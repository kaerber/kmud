using System;
using Microsoft.Practices.Unity;

using Kaerber.MUD.Server;
using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class AffectTest : BaseEntityTest {
        [Test]
        public void ConstructorTest_AffectData() {
            var data = new AffectInfo { Name = "test affect" };
            var affect = new Affect( data );
            Assert.AreEqual( data.Name, affect.Name );
        }

        [Test]
        public void DurationTest() {
            World.Instance = new World();
            var container = UnityConfigurator.Configure();
            container.RegisterInstance( new Clock( 0 ) );
            World.Instance.Initialize( container );

            var affect = new Affect { Duration = 20 };
            Assert.AreEqual( 20, affect.Duration );
            World.Instance.Pulse( 10*10000 );
            Assert.AreEqual( 10, affect.Duration );
        }

        [Test]
        public void TargetTest() {
            var data = new AffectInfo {
                Name = "horde_room",
                Target = AffectTarget.Room
            };

            var affect = new Affect( data );

            Assert.AreEqual( data.Target, affect.Target );
        }

        [Test]
        public void ReceiveEventTest() {
            var data = new AffectInfo {
                Handlers = new HandlerSet {
                    { "test_event", "10/0" },
                    { "test_event_correct", "10/1" }
                }
            };
            var affect = new Affect( data );
            var ch = new Character();

            Assert.Throws<DivideByZeroException>( () => 
                affect.ReceiveEvent( Event.Create( "test_event" ), ch )
            );

            affect.ReceiveEvent( Event.Create( "test_event_correct" ), ch );
        }
    }
}
