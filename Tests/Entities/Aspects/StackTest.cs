using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class StackTest
    {
        [Test]
        public void AddTest()
        {
            var stack = new Stack( 5 );
            var added = stack.Add( 1 );
            Assert.AreEqual( 1, stack.Count );
            Assert.AreEqual( 1, added );

            added = stack.Add( 1 );
            Assert.AreEqual( 2, stack.Count );
            Assert.AreEqual( 1, added );

            added = stack.Add( 4 );
            Assert.AreEqual( 5, stack.Count );
            Assert.AreEqual( 3, added );

            added = stack.Add( 1 );
            Assert.AreEqual( 5, stack.Count );
            Assert.AreEqual( 0, added );

            stack = new Stack();
            added = stack.Add( 1 );
            Assert.AreEqual( 1, stack.Count );
            Assert.AreEqual( 1, added );

            added = stack.Add( 100 );
            Assert.AreEqual( 101, stack.Count );
            Assert.AreEqual( 100, added );
        }

        [Test]
        public void IsFullTest()
        {
            var stack1 = new Stack( 5 );
            stack1.Add( 1 );
            Assert.IsFalse( stack1.IsFull );

            var stack2 = new Stack( 5 );
            stack2.Add( 5 );
            Assert.IsTrue( stack2.IsFull );

            var stack3 = new Stack();
            stack3.Add( 5 );
            Assert.IsFalse( stack3.IsFull );

        }
    }
}
