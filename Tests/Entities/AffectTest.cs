using System;

using NUnit.Framework;

using Kaerber.MUD.Entities;

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
            World.Instance = new World { Time = 0 };

            var affect = new Affect { Duration = 20 };
            Assert.AreEqual( 20, affect.Duration );
            World.Instance.Time = 10;
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
