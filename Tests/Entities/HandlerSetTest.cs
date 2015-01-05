using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class HandlerSetTest
    {
        [Test]
        public void AddTest()
        {
            var handlerSet = new HandlerSet();
            handlerSet.Add( new KeyValuePair<string, string>( "ch_says_text", "import clr" ) );

            Assert.AreEqual( 1, handlerSet.Count );
            Assert.AreEqual( handlerSet[ "ch_says_text" ], "import clr" );
        }

        [Test]
        public void ClearTest()
        {
            var handlerSet = new HandlerSet();
            handlerSet.Add( "ch_says_text", "import clr" );
            Assert.AreEqual( 1, handlerSet.Count );

            handlerSet.Clear();
            Assert.AreEqual( 0, handlerSet.Count );
        }

        [Test]
        public void ContainsTest()
        {
            var handlerSet = new HandlerSet();
            handlerSet.Add( "ch_says_text", "import clr" );

            Assert.IsTrue( handlerSet.Contains( "ch_says_text" ) );
            Assert.IsFalse( handlerSet.Contains( "ch_can_die" ) );
            Assert.IsTrue(
                handlerSet.Contains(
                    new KeyValuePair<string, string>( "ch_says_text", "import clr" )
                )
            );
            Assert.IsFalse(
                handlerSet.Contains(
                    new KeyValuePair<string, string>( "ch_can_die", "import clr" )
                )
            );
            Assert.IsFalse(
                handlerSet.Contains(
                    new KeyValuePair<string, string>( "ch_says_text", "import clr\n" )
                )
            );
        }

        [Test]
        public void CopyToTest()
        {
            var handlerSet = new HandlerSet();
            handlerSet.Add( "ch_says_text", "import clr" );

            Assert.Throws<NotImplementedException>(
                () => handlerSet.CopyTo( new string[1], 0 ) );
            var array = new KeyValuePair<string, string>[1];
            handlerSet.CopyTo( array, 0 );

            Assert.AreEqual( "ch_says_text", array[ 0 ].Key );
            Assert.AreEqual( "import clr", array[ 0 ].Value );
        }

        [Test]
        public void ExecuteTest()
        {
            var handlerSet = new HandlerSet { { "ch_says_text", "import clr\nret_val = 2" } };

            var e = Event.Create( "ch_says_text", EventReturnMethod.None );
            handlerSet.Execute( e );
            Assert.AreEqual( 2, e.ReturnValue );
        }

        [Test]
        public void ValuesTest()
        {
            var handlerSet = new HandlerSet { { "ch_says_text", "import clr" } };

            Assert.AreEqual( 1, handlerSet.Values.Count );
            Assert.AreEqual( 1, ( ( IDictionary )handlerSet ).Values.Count );
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var set = new HandlerSet();
            var enumerator = set.GetEnumerator();
            var enumerator2 = ( ( IDictionary )set ).GetEnumerator();
            var enumerator3 = ( ( IEnumerable )set ).GetEnumerator();
        }

        [Test]
        public void IsFixedSizeTest()
        {
            Assert.AreEqual( false, new HandlerSet().IsFixedSize );
        }

        [Test]
        public void IsReadnlyTest()
        {
            Assert.AreEqual( false, new HandlerSet().IsReadOnly );
        }

        [Test]
        public void IsSynchronizedTest()
        {
            Assert.AreEqual( false, new HandlerSet().IsSynchronized );
        }

        [Test]
        public void KeysTest()
        {
            var handlerSet = new HandlerSet { { "ch_says_text", "import clr" } };

            Assert.AreEqual( 1, handlerSet.Keys.Count );
        }

        [Test]
        public void RemoveTest()
        {
            var handlerSet = new HandlerSet { { "ch_says_text", "import clr" } };

            handlerSet.Remove( new KeyValuePair<string, string>( "ch_says_text", "import clr" ) );
            Assert.AreEqual( 0, handlerSet.Count );

            handlerSet.Add( "ch_says_text", "import clr" );

            handlerSet.Remove( "ch_says_text" );
            Assert.AreEqual( 0, handlerSet.Count );

            handlerSet.Add( "ch_says_text", "import clr" );

            handlerSet.Remove( ( object )"ch_says_text" );
            Assert.AreEqual( 0, handlerSet.Count );

        }

        [Test]
        public void SyncRootTest()
        {
            Assert.IsNotNull( new HandlerSet().SyncRoot );
        }

        [Test]
        public void TryGetValueTest()
        {
            var handlerSet = new HandlerSet { { "ch_says_text", "import clr" } };

            string value;
            Assert.IsTrue( handlerSet.TryGetValue( "ch_says_text", out value ) );
            Assert.AreEqual( "import clr", value );
        }
    }
}
