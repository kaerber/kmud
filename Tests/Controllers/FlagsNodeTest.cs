using System;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers
{
    [TestFixture]
    public class FlagsNodeTest
    {
        [SetUp]
        public void SetUp() {
            _mockView = new Mock<ICharacterView>();
        }

        public void IndexerTest()
        {
            var node = HierarchyNode.CreateNode( null, AffectFlags.Multiple | AffectFlags.NoDeath, null );
            Assert.IsInstanceOf<FlagsNode>( node );

            Assert.Throws<NotSupportedException>( () => { var value = node["0"]; } );
        }

        [Test]
        public void SetMemberTest()
        {
            HierarchyNode node = HierarchyNode.CreateNode( null, AffectFlags.Multiple | AffectFlags.NoDeath, null );
            Assert.IsInstanceOf<FlagsNode>( node );

            Assert.Throws<NotSupportedException>( () => node.SetMember( "key", "0" ) );
        }

        [Test]
        public void SetTest()
        {
            var affectData = new AffectInfo { Name = "test_affect", Target = AffectTarget.Character, Flags = AffectFlags.Multiple };

            var root = HierarchyNode.CreateNode( null, affectData, null );
            Assert.IsInstanceOf<ComplexNode>( root );
            var node = root["flag"];
            Assert.IsInstanceOf<FlagsNode>( node );

            node.Set( "multiple nodeath", _mockView.Object );
            Assert.IsTrue( affectData.Flags.HasFlag( AffectFlags.NoDeath ) );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.Multiple ) );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.Hidden ) );
        }

        [Test]
        public void AddTest()
        {
            var affectData = new AffectInfo { Name = "test_affect", Target = AffectTarget.Character, Flags = AffectFlags.Multiple };

            var root = HierarchyNode.CreateNode( null, affectData, null );
            Assert.IsInstanceOf<ComplexNode>( root );
            var node = root["flag"];
            Assert.IsInstanceOf<FlagsNode>( node );

            node.Add( "multiple nodeath", _mockView.Object );
            Assert.IsTrue( affectData.Flags.HasFlag( AffectFlags.NoDeath ) );
            Assert.IsTrue( affectData.Flags.HasFlag( AffectFlags.Multiple ) );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.Hidden ) );
        }

        [Test]
        public void RemoveTest()
        {
            var affectData = new AffectInfo { Name = "test_affect", Target = AffectTarget.Character, Flags = AffectFlags.Multiple };

            var root = HierarchyNode.CreateNode( null, affectData, null );
            Assert.IsInstanceOf<ComplexNode>( root );
            var node = root["flag"];
            Assert.IsInstanceOf<FlagsNode>( node );

            node.Remove( "multiple nodeath", _mockView.Object );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.NoDeath ) );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.Multiple ) );
            Assert.IsFalse( affectData.Flags.HasFlag( AffectFlags.Hidden ) );
        }

        private Mock<ICharacterView> _mockView;
    }
}
