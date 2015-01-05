using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class ComplexAspectTest : BaseEntityTest {
        [Test]
        public void AddRemoveTest() {
            var ch = new Character();
            var complex = AspectFactory.Complex();
            complex.Host = ch;
            var test = AspectFactory.Test();
            complex.test = test;
            Assert.IsNotNull( complex.test );
            Assert.IsNotNull( complex["test"] );

            Assert.IsNull( complex["notExists"] );
            
            Assert.AreEqual( complex.test, test );
            Assert.AreEqual( complex["test"], test );
            
            Assert.AreEqual( test._host, ch );

            complex.Remove( "test" );
            Assert.IsNull( complex["test"] );

            test._host = null;

            complex["test"] = test;
            Assert.IsNotNull( complex.test );
            Assert.IsNotNull( complex["test"] );

            Assert.IsNull( complex["notExists"] );
            
            Assert.AreEqual( complex.test, test );
            Assert.AreEqual( complex["test"], test );
            
            Assert.AreEqual( test._host, ch );
        }

        [Test]
        public void AddWithSetHostTest() {
            var ch = new Character { ShortDescr = "test" };
            var complex = AspectFactory.Complex();
            complex.Host = ch;
            var test = AspectFactory.Test();
            complex.Add("test", test);

            Assert.AreEqual( ch, complex.Host );
            Assert.AreEqual( ch, test.Host );
        }

        [Test]
        public void SetHostTest() {
            var ch = new Character { ShortDescr = "test" };
            var complex = AspectFactory.Complex();
            var test = AspectFactory.Test();
            complex.Add( "test", test );

            complex.Host = ch;
            Assert.AreEqual( ch, complex.Host );
            Assert.AreEqual( ch, test.Host );
        }

        [Test]
        public void ReceiveEventTest() {
            var ch = new Character();
            var complex = AspectFactory.Complex();
            complex.Host = ch;
            var test = AspectFactory.Test();
            complex.Add("test", test);

            var e = Event.Create( "test_completed", EventReturnMethod.And );
            complex.ReceiveEvent( e );
            Assert.IsTrue( e.ReturnValue );
        }

        [Test]
        public void CloneTest() {
            var mockDestCombat = new Mock<IAspect>();
            var mockSrcCombat = new Mock<IAspect>();
            mockSrcCombat.Setup( srcCombat => srcCombat.Clone() )
                .Returns( mockDestCombat.Object );

            var mockDestStats = new Mock<IAspect>();
            var mockSrcStats = new Mock<IAspect>();
            mockSrcStats.Setup( srcStats => srcStats.Clone() )
                .Returns( mockDestStats.Object );

            var complex = AspectFactory.Complex();
            complex.combat = mockSrcCombat.Object;
            complex.stats = mockSrcStats.Object;

            var clone = complex.Clone();

            Assert.IsNotNull( clone );
            mockSrcCombat.VerifyAll();
            mockDestCombat.VerifyAll();
        }

        [Test]
        public void DeserializeTest() {
            var complex = AspectFactory.Complex();
            complex.stats = AspectFactory.Stats();
            var data = new Dictionary<string, object> {
                { "stats", new Dictionary<string, object>() }
            };

            complex.Deserialize( data );

            Assert.IsNotNull( complex.stats );
        }

        [Test]
        public void SerializeTest() {
            var complex = AspectFactory.Complex();
            complex.stats = AspectFactory.Stats();
            complex.health = AspectFactory.Health();

            var data = complex.Serialize();
            Assert.IsNotNull( data["stats"] );
            Assert.IsNotNull( data["health"] );
        }
    }
}
