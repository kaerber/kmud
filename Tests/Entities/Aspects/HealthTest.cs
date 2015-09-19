using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class HealthTest : BaseEntityTest {
        [Test]
        public void MaxHealthLaunchesQueryEvent() {
            var eventList = new List<Event>();

            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( eventList.Add );

            var health = AspectFactory.Health();
            health.Host = mockSelf.Object;

            var max = health.Max;

            mockSelf.VerifyAll();
            Assert.AreEqual( "query_max_health", eventList[0].Name );
        }

        [Test]
        public void DeserializeReturnsSelfTest() {
            var health = AspectFactory.Health();
            health.Restore = new Action( () => {} );
            var self = health.Deserialize( JsonConvert.DeserializeObject( "{}" ) );
            Assert.IsNotNull( self );
        }


        [Test]
        public void CloneTest() {
            var health = AspectFactory.Health();
            var clone = health.Clone();

            Assert.IsNotNull( clone );
        }
    }
}
