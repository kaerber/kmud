using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{

    [TestFixture( Category = "acceptance" )]
    public class MaxHealthAcceptance
    {
        /// <summary>
        /// Char is created with all the default stats.
        /// Max health is not null.
        /// </summary>
        [Test]
        public void DefaultCharIsCreatedWithNonZeroHealth()
        {
            var ch = new Character();
            Assert.Greater( ch.Health.Max, 0 );
        }
    }
}
