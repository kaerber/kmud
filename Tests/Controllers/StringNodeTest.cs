using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class StringNodeTest : BaseEntityTest {
        [SetUp]
        public void Setup() {
            _mockView = new Mock<ICharacterView>();
        }

        [Test]
        public void IndexerTest() {
            const string str = "my\ntest\nstring";
            var node = HierarchyNode.CreateNode( null, str, null );

            Assert.IsInstanceOf<StringNode>( node["0"] );

            Assert.AreEqual( null, node["-1"] );
            Assert.AreEqual( "my", node["0"].Value );
            Assert.AreEqual( "test", node["1"].Value );
            Assert.AreEqual( "string", node["2"].Value );
            Assert.AreEqual( null, node["3"] );

            Assert.AreEqual( null, node["1"]["0"] );

            Assert.AreEqual( null, node["xxx"] );
        }

        [Test]
        public void SetTest() {
            var room = new Room();
            var node = HierarchyNode.CreateNode( null, room, null );
            node[ "descr" ].Set( "test\nstring", _mockView.Object );
            Assert.AreEqual( "test\nstring", room.Description );

            node[ "descr" ][ "0" ].Set( "my\ntest", _mockView.Object );
            Assert.AreEqual( "my\ntest\nstring", room.Description );
        }

        [Test]
        public void AddTest() {
            var room = new Room { Description = "test\nstring" };
            var node = HierarchyNode.CreateNode( null, room, null );

            node[ "desc" ].Add( "flaming june", _mockView.Object );
            Assert.AreEqual( "test\nstring\nflaming june", room.Description );

            node[ "descr" ][ "0" ].Add( "case", _mockView.Object );
            Assert.AreEqual( "test\ncase\nstring\nflaming june", room.Description );
        }

        [Test]
        public void RemoveTest() {
            var room = new Room { Description = "my\ntest\nstring" };
            var node = HierarchyNode.CreateNode( null, room, null );

            node[ "desc" ].Remove( "1", _mockView.Object );
            Assert.AreEqual( "my\nstring", room.Description );
        }

        private Mock<ICharacterView> _mockView;
    }
}
