using System;
using System.Collections.Generic;

using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Tests.Controllers
{
    [TestFixture]
    public class ListNodeTest
    {
        [SetUp]
        public void SetUp() {
            _mockView = new Mock<ICharacterView>();
        }

        [Test]
        public void IndexerTest()
        {
            var list = new List<string> { "value" };
            var node = HierarchyNode.CreateNode( null, list, null );

            Assert.IsInstanceOf<StringNode>( node["0"] );
            Assert.AreEqual( "value", node["0"].Value );

            Assert.AreEqual( null, node["xxx"] );
            Assert.AreEqual( null, node["-1"] );
            Assert.AreEqual( null, node["1"] );
        }

        [Test]
        public void SetTest()
        {
            var node = HierarchyNode.CreateNode( null, new List<Apply>(), null );
            Assert.IsInstanceOf<ListNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Set( "list", _mockView.Object ) );
        }

        [Test]
        public void SetMemberTest()
        {
            var list = new List<Apply>();
            var node = HierarchyNode.CreateNode( null, list, null );
            Assert.IsInstanceOf<ListNode>( node );

            node.Add( string.Empty, _mockView.Object );
            Assert.AreEqual( 1, list.Count );

            node.SetMember( "0", new Apply { HP = 23 } );
            Assert.AreEqual( 23, ( int )node["0"]["hp"].Value );
        }

        [Test]
        public void AddTest()
        {
            var list = new List<Apply>();
            var node = HierarchyNode.CreateNode( null, list, null );
            Assert.IsInstanceOf<ListNode>( node );

            node.Add( "0", _mockView.Object );
            Assert.AreEqual( 1, list.Count );
        }

        [Test]
        public void RemoveTest()
        {
            var list = new List<Apply> { new Apply() };
            HierarchyNode node = HierarchyNode.CreateNode( null, list, null );
            Assert.IsInstanceOf<ListNode>( node );

            node.Remove( "0", _mockView.Object );
            Assert.AreEqual( 0, list.Count );
        }

        private Mock<ICharacterView> _mockView;
    }
}
