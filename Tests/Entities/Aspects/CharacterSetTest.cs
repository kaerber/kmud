using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class CharacterSetTest
    {
        [Test]
        public void WhereTest()
        {
            var chA = new Character { ShortDescr = "a" };
            var chB = new Character { ShortDescr = "b" };
            var chC = new Character { ShortDescr = "c" };

            var set = new CharacterSet( new List<Character> { chA, chB, chC } );
            var result = set.Where( ch => ch.ShortDescr != "b" );

            Assert.AreEqual( 2, result.Count );
            Assert.IsTrue( result.Contains( chA ) );
            Assert.IsTrue( result.Contains( chC ) );
        }
    }
}
