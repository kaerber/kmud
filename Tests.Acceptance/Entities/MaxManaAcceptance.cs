using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{

    [TestFixture( Category = "acceptance" )]
    public class MaxManaAcceptance
    {
        /// <summary>
        /// Char is created with all the default stats.
        /// Max health is not null.
        /// </summary>
        [Test]
        public void DefaultCharIsCreatedWithNonZeroMana()
        {
            var ch = new Character();
            Assert.Greater( ch.Mana.Max, 0 );
        }
    }
}
