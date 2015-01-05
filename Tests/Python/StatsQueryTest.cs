using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Python {
    [TestFixture]
    public class StatsQueryTest {
        [SetUp]
        public void SetUp() {
            _query = AspectFactory.StatQuery();
        }

        [Test]
        public void ConstitutionIsAdditive() {
            _query.Con = Value1;
            _query.Con = Value2;

            Assert.AreEqual( Value1 + Value2, _query.Con );
        }

        [Test]
        public void IntellectIsAdditive() {
            _query.Int = Value1;
            _query.Int = Value2;

            Assert.AreEqual( Value1 + Value2, _query.Int );
        }

        [Test]
        public void WoundsValueIsReplaced() {
            _query.Wounds = Value1;
            _query.Wounds = Value2;

            Assert.AreEqual( Value2, _query.Wounds );
        }

        [Test]
        public void ManaSpentIsReplaced() {
            _query.ManaSpent = Value1;
            _query.ManaSpent = Value2;

            Assert.AreEqual( Value2, _query.ManaSpent );
        }

        [Test]
        public void TestMaxHpCalculation() {
            _query.Con = Value1;

            Assert.AreEqual( 25*Value1, _query.MaxHP );
        }

        [Test]
        public void TestHpCalculation() {
            _query.Con = Value1;
            _query.Wounds = Value2;

            Assert.AreEqual( 25*Value1 - Value2, _query.HP );
        }

        [Test]
        public void TestMaxMpCalculation() {
            _query.Int = Value1;
            
            Assert.AreEqual( 25*Value1, _query.MaxMP );
        }

        [Test]
        public void TestMpCalculation() {
            _query.Int = Value1;
            _query.ManaSpent = Value2;

            Assert.AreEqual( Value1*25 - Value2, _query.MP );
        }

        private dynamic _query;

        private const int Value1 = 17;
        private const int Value2 = 26;
    }
}
