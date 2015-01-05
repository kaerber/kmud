using System.Collections.Generic;

using Kaerber.MUD.Tests.Entities;

using NUnit.Framework;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class HierarchyNodeTest : BaseEntityTest {
        [Test]
        public void CreateNodeTest()
        {
            // NumberNode
            var node = HierarchyNode.CreateNode( null, 10, null );
            Assert.IsInstanceOf<NumberNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // StringNode
            node = HierarchyNode.CreateNode( null, "test\nmultiline\nstring", null );
            Assert.IsInstanceOf<StringNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // ComplexNode
            node = HierarchyNode.CreateNode( null, new Character(), null );
            Assert.IsInstanceOf<ComplexNode>( node );

            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // ListNode
            node = HierarchyNode.CreateNode( null, new List<string>(), null );
            Assert.IsInstanceOf<ListNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // DictionaryNode
            node = HierarchyNode.CreateNode( null, new Dictionary<string, object>(), null );
            Assert.IsInstanceOf<DictionaryNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // EnumNode
            node = HierarchyNode.CreateNode( null, WearLocation.Body, null );
            Assert.IsInstanceOf<EnumNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<FlagsNode>( node );

            // FlagsNode
            node = HierarchyNode.CreateNode( null, AffectFlags.Multiple|AffectFlags.Hidden, null );
            Assert.IsInstanceOf<FlagsNode>( node );

            Assert.IsNotInstanceOf<ComplexNode>( node );
            Assert.IsNotInstanceOf<ListNode>( node );
            Assert.IsNotInstanceOf<DictionaryNode>( node );
            Assert.IsNotInstanceOf<StringNode>( node );
            Assert.IsNotInstanceOf<NumberNode>( node );
            Assert.IsNotInstanceOf<EnumNode>( node );
        }

        [Test]
        public void IsNumberTest() {
            Assert.IsTrue( HierarchyNode.IsNumber( 10.GetType() ) );

            Assert.IsFalse( HierarchyNode.IsNumber( "test".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsNumber( new List<string>().GetType() ) );
            Assert.IsFalse( HierarchyNode.IsNumber( "test\nmultiline\nstring".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsNumber( new Dictionary<string, object>().GetType() ) );
        }

        [Test]
        public void IsStringTest() {
            Assert.IsTrue( HierarchyNode.IsString( "test".GetType() ) );
            Assert.IsTrue( HierarchyNode.IsString( "test\nmultiline\nstring".GetType() ) );

            Assert.IsFalse( HierarchyNode.IsString( 10.GetType() ) );
            Assert.IsFalse( HierarchyNode.IsString( new List<string>().GetType() ) );
            Assert.IsFalse( HierarchyNode.IsString( new Dictionary<string, object>().GetType() ) );
        }

        [Test]
        public void IsListTest() {
            Assert.IsTrue( HierarchyNode.IsList( new List<string>().GetType() ) );

            Assert.IsFalse( HierarchyNode.IsList( 10.GetType() ) );
            Assert.IsFalse( HierarchyNode.IsList( "test".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsList( "test\nmultiline\nstring".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsList( new Dictionary<string, object>().GetType() ) );
        }

        [Test]
        public void IsDictionaryTest() {
            Assert.IsTrue( HierarchyNode.IsDictionary( new Dictionary<string, object>().GetType() ) );

            Assert.IsFalse( HierarchyNode.IsDictionary( 10.GetType() ) );
            Assert.IsFalse( HierarchyNode.IsDictionary( "test".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsDictionary( "test\nmultiline\nstring".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsDictionary( new List<string>().GetType() ) );

            Assert.IsTrue( HierarchyNode.IsDictionary( new HandlerSet().GetType() ) );
        }

        [Test]
        public void IsComplexObjectTest() {
            Assert.IsTrue( HierarchyNode.IsComplexObject( new Apply().GetType() ) );
            Assert.IsTrue( HierarchyNode.IsComplexObject( new Exit( "west", string.Empty ).GetType() ) );
            Assert.IsTrue( HierarchyNode.IsComplexObject( new Character().GetType() ) );
            Assert.IsTrue( HierarchyNode.IsComplexObject( new MLFunction().GetType() ) );
            Assert.IsTrue( HierarchyNode.IsComplexObject( new SkillData().GetType() ) );

            Assert.IsFalse( HierarchyNode.IsComplexObject( 10.GetType() ) );
            Assert.IsFalse( HierarchyNode.IsComplexObject( "test".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsComplexObject( "test\nmultiline\nstring".GetType() ) );
            Assert.IsFalse( HierarchyNode.IsComplexObject( new List<string>().GetType() ) );
            Assert.IsFalse( HierarchyNode.IsComplexObject( new Dictionary<string, object>().GetType() ) );
        }

        [Test]
        public void PathTest() {
            var ch = new Character();

            var node = HierarchyNode.CreateNode( null, ch, null );
            Assert.AreEqual( "Stats.Armor", node["stat"]["arm"].Path );
        }
    }
}
