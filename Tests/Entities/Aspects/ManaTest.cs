using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class ManaTest : BaseEntityTest {
        [Test]
        public void MaxManaLaunchesQueryEvent() {
            var eventList = new List<Event>();

            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( eventList.Add );

            var mana = AspectFactory.Mana();
            mana.Host = mockSelf.Object;

            var max = mana.Max;

            mockSelf.VerifyAll();
            Assert.AreEqual( "query_max_mana", eventList[0].Name );
        }

        [Test]
        public void CloneTest() {
            var mana = AspectFactory.Mana();
            var clone = mana.Clone();
            Assert.IsNotNull( clone );
        }

        [Test]
        public void DeserializeReturnsSelf() {
            var mana = AspectFactory.Mana();
            var self = mana.Deserialize( new Dictionary<string, object> { { "Value", 100 } } );
            Assert.IsNotNull( self );
        }
    }
}
