using System;

using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Controllers
{
    [TestFixture]
    public class EnumNodeTest : BaseEntityTest {
        [SetUp]
        public void Setup() {
            _mockView = new Mock<ICharacterView>();
        }

        [Test]
        public void IndexerTest() {
            var node = HierarchyNode.CreateNode( null, WearLocation.Body, null );
            Assert.IsInstanceOf<EnumNode>( node );

            Assert.Throws<NotSupportedException>( () => { var value = node["0"]; } );
        }

        [Test]
        public void AddTest() {
            var node = HierarchyNode.CreateNode( null, WearLocation.Body, null );
            Assert.IsInstanceOf<EnumNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Add( "1", _mockView.Object ) );
        }

        [Test]
        public void SetMemberTest() {
            var node = HierarchyNode.CreateNode( null, WearLocation.Body, null );
            Assert.IsInstanceOf<EnumNode>( node );

            Assert.Throws<NotSupportedException>( () => node.SetMember( "key", "0" ) );
        }

        [Test]
        public void SetTest() {
            var item = new Item();

            var root = HierarchyNode.CreateNode( null, item, null );
            Assert.IsInstanceOf<ComplexNode>( root );
            var node = root["wear"];
            Assert.IsInstanceOf<EnumNode>( node );

            node.Set( "body", _mockView.Object );
            Assert.AreEqual( WearLocation.Body, item.WearLoc );
        }

        [Test]
        public void RemoveTest() {
            var node = HierarchyNode.CreateNode( null, WearLocation.Body, null );
            Assert.IsInstanceOf<EnumNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Remove( "1", _mockView.Object ) );
        }

        private Mock<ICharacterView> _mockView;
    }
}
