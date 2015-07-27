using System;
using System.Collections.Generic;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class DictionaryNodeTest : BaseEntityTest {
        [SetUp]
        public void Setup() {
            _mockView = new Mock<ICharacterView>();
        }

        [Test]
        public void IndexerTest()
        {
            var dict = new Dictionary<string, string> { { "key", "value" } };
            var node = HierarchyNode.CreateNode( null, dict, null );

            Assert.IsInstanceOf<StringNode>( node["key"] );
            Assert.AreEqual( "value", node["key"].Value );

            Assert.AreEqual( null, node["xxx"] );
        }

        [Test]
        public void SetTest() {
            var dictionary = new Dictionary<string, Character>();
            var node = HierarchyNode.CreateNode( null, dictionary, null );
            Assert.IsInstanceOf<DictionaryNode>( node );
            Assert.Throws<NotSupportedException>( () => node.Set( "dictionary", _mockView.Object ) );
        }

        [Test]
        public void SetMemberTest() {
            var dictionary = new Dictionary<string, Character>();
            var node = HierarchyNode.CreateNode( null, dictionary, null );
            Assert.IsInstanceOf<DictionaryNode>( node );

            node.Add( "test", _mockView.Object );
            Assert.AreEqual( 1, dictionary.Count );

            node.SetMember( "test", new Character( new CharacterCore() ) { ShortDescr = "test char" } );
            Assert.AreEqual( "test char", ( string )node["test"]["short"].Value );
        }

        [Test]
        public void AddTest() {
            var dictionary = new Dictionary<string, Character>();
            var node = HierarchyNode.CreateNode( null, dictionary, null );
            Assert.IsInstanceOf<DictionaryNode>( node );

            node.Add( "test", _mockView.Object );
            Assert.AreEqual( 1, dictionary.Count );
        }

        [Test]
        public void RemoveTest() {
            var dictionary = new Dictionary<string, Character> { { "test", new Character( new CharacterCore() ) } };
            var node = HierarchyNode.CreateNode( null, dictionary, null );
            Assert.IsInstanceOf<DictionaryNode>( node );

            node.Remove( "test", _mockView.Object );
            Assert.AreEqual( 0, dictionary.Count );
        }

        private Mock<ICharacterView> _mockView;
    }
}
