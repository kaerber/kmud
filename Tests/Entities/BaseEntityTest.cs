using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Server;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    public class BaseEntityTest {
        [TestFixtureSetUp]
        public virtual void FixtureSetup() {
            Launcher.InitializeML();
            UnityConfigurator.Configure();
        }

        public static void AssertSequenceEqual<T>( IEnumerable<T> expected, IEnumerable<T> actual ) {
            var lexpected = expected.ToList();
            var lactual = actual.ToList();
            Assert.IsTrue( lexpected.SequenceEqual( lactual ),
                           $"Expected {FormatSequence( lexpected )}, but was {FormatSequence( lactual )}" );
        }

        public static string FormatSequence<T>( IList<T> sequence ) {
            return "[" + string.Join( ", ", sequence.Select( item => item.ToString() ) ) + "]";
        }
    }
}
