using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class EventTest : BaseEntityTest {
        [Test]
        public void SumReturnMethodSummarizesValues() {
            var e = Event.Create( "sum_test", EventReturnMethod.Sum );
            Assert.AreEqual( 0, e.ReturnValue );

            e.ReturnValue = 100;
            Assert.AreEqual( 100, e.ReturnValue );
        }
    }
}
