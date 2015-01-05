using System;
using System.Collections.Generic;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests {
    [TestFixture]
    public class ExtensionMethodsTest {
       [Test]
        public void MatchTest() {
            var list = new List<string> { "test", "match1", "match", "mat" };
            Assert.AreEqual( "def", list.Match( " ", "def" ) );
            Assert.AreEqual( "def", list.Match( "key", "def" ) );
            Assert.AreEqual( "test", list.Match( "test", "def" ) );
            Assert.AreEqual( "match1", list.Match( "ma", "def" ) );
            Assert.AreEqual( "match", list.Match( "match", "def" ) );
            Assert.AreEqual( "mat", list.Match( "mat", "def" ) );
        }

        [Test]
        public void NotNullTest() {
            Func<List<int>> funcRef = null;
            Assert.IsNotNull( funcRef.NotNull() );
            Assert.IsNotNull( funcRef.NotNull()() );

            Func<int> funcVal = null;
            Assert.IsNotNull( funcVal.NotNull() );
            Assert.AreEqual( 0, funcVal.NotNull()() );
        }
    }
}
