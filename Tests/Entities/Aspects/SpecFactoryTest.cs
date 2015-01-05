using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class SpecFactoryTest
    {
        [Test]
        public void TestGetWarrior()
        {
            var spec = SpecFactory.Warrior;
            Assert.IsNotNull( spec );
            Assert.IsNotNull( spec.AutoAttack );
        }
    }
}
