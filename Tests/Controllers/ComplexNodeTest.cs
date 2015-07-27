using System;
using System.Reflection;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Views;

using Moq;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class ComplexNodeTest {
        [SetUp]
        public void Setup() {
            _mockView = new Mock<ICharacterView>();
        }

        [Test]
        public void IndexerTest() {
            var ch = new Character( new CharacterCore() );

            var node = HierarchyNode.CreateNode( null, ch, null );

            Assert.IsInstanceOf<PythonNode>( node["stat"] );
            Assert.AreEqual( ch.Stats, node["stat"].Value );

            Assert.AreEqual( null, node["xxx"] );

            var item = new Item { Weapon = AspectFactory.Weapon() };

            var itemNode = HierarchyNode.CreateNode( null, item, null );
            Assert.IsInstanceOf<PythonNode>( itemNode["weap"] );
        }

        [Test]
        public void GetPropertyTest() {
            var testedMethod = typeof( ComplexNode ).GetMethod(
                "GetProperty",
                BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public );
            Assert.NotNull( testedMethod );

            var item = new Item { Weapon = AspectFactory.Weapon() };
            var itemNode = HierarchyNode.CreateNode( null, item, null );

            var value = testedMethod.Invoke( itemNode, new object[] { "weap" } );
            Assert.NotNull( value );
            Assert.IsInstanceOf<PropertyInfo>( value );
        }

        [Test]
        public void SetTest() {
            var item = new Item();
            var node = HierarchyNode.CreateNode( null, item, null );
            Assert.IsInstanceOf<ComplexNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Set( "test", _mockView.Object ) );
        }

        [Test]
        public void AddTest() {
            var item = new Item();
            var node = HierarchyNode.CreateNode( null, item, null );
            Assert.IsInstanceOf<ComplexNode>( node );

            Assert.IsNull( item.Weapon );
            node.Add( "weapon", _mockView.Object );
            Assert.IsNotNull( item.Weapon );
        }

        [Test]
        public void RemoveTest() {
            var item = new Item { Weapon = AspectFactory.Weapon() };
            var node = HierarchyNode.CreateNode( null, item, null );
            Assert.IsInstanceOf<ComplexNode>( node );

            Assert.IsNotNull( item.Weapon );
            node.Remove( "weapon", _mockView.Object );
            Assert.IsNull( item.Weapon );
        }

        private Mock<ICharacterView> _mockView;
    }
}
