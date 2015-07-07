using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Server;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    public class BaseEntityTest {
        [TestFixtureSetUp]
        public virtual void FixtureSetup() {
            Launcher.InitializeML();
            World.Serializer = Launcher.InitializeSerializer();
        }

        public static void AssertSequenceEqual<T>( IEnumerable<T> expected, IEnumerable<T> actual ) {
            var lexpected = expected.ToList();
            var lactual = actual.ToList();
            Assert.IsTrue( lexpected.SequenceEqual( lactual ),
                           string.Format( "Expected {0}, but was {1}",
                                          FormatSequence( lexpected ),
                                          FormatSequence( lactual ) ) );
        }

        public static string FormatSequence<T>( IEnumerable<T> sequence ) {
            return "[" + string.Join( ", ", sequence.Select( item => item.ToString() ) ) + "]";
        }
    }
}
