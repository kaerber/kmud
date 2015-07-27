using System;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class NumberNodeTest : BaseEntityTest {
        [SetUp]
        public void TestFixtureSetup() {
            _view = new Mock<ICharacterView>().Object;
        }

        [Test]
        public void IndexerTest() {
            var node = HierarchyNode.CreateNode( null, 10, null );
            Assert.IsInstanceOf<NumberNode>( node );
            Assert.Throws<NotSupportedException>( () => { var value = node["0"]; } );
        }

        [Test]
        public void AddTest() {
            var node = HierarchyNode.CreateNode( null, 10, null );
            Assert.IsInstanceOf<NumberNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Add( "1", _view ) );
        }

        [Test]
        public void SetMemberTest() {
            var node = HierarchyNode.CreateNode( null, 10, null );
            Assert.IsInstanceOf<NumberNode>( node );
            Assert.Throws<NotSupportedException>( () => node.SetMember( "key", "0" ) );
        }

        [Test]
        public void SetTest() {
            var ch = new Character( new CharacterCore() );

            var root = HierarchyNode.CreateNode( null, ch, null );
            Assert.IsInstanceOf<ComplexNode>( root );
            var node = root["stat"]["armor"];
            Assert.IsInstanceOf<NumberNode>( node );

            node.Set( "10", _view );
            Assert.AreEqual( 10, ch.Stats.Armor );
        }

        [Test]
        public void RemoveTest() {
            var node = HierarchyNode.CreateNode( null, 10, null );
            Assert.IsInstanceOf<NumberNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Remove( "1", _view ) );
        }

        private ICharacterView _view;
    }
}
