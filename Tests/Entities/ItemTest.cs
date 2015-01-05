using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Tests.Entities.Aspects;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class ItemTest : BaseEntityTest {
        private static Item CreateTestItem() {
            var item = new Item {
                Id = "ser_item",
                ShortDescr = "Serialized Item",
                MaxCount = 20
            };
            item.AddQuantity( 5 );
            return item;
        }

        private static string SerializeItem( Item item ) {
            return World.Serializer.Serialize( item );
        }


        [Test]
        public void WearLocStringsFullSet() {
            foreach( var x in typeof( WearLocation ).GetEnumValues() ) {
                Assert.Contains( x, WearLocationStrings.Strings.Keys );
                Assert.False( string.IsNullOrWhiteSpace( WearLocationStrings.Strings[( WearLocation )x].Name ),
                    string.Format( "No WearLocationString-Name for WearLocation.{0}", ( WearLocation )x ) );
                Assert.False( string.IsNullOrWhiteSpace( WearLocationStrings.Strings[( WearLocation )x].Prefix ),
                    string.Format( "No WearLocationString-Prefix for WearLocation.{0}", ( WearLocation )x ) );
            }
        }

        [Test]
        public void Serialize() {
            var mockStats = new Mock<IAspect>();
            mockStats.Setup( stats => stats.Serialize() );

            var mockWeapon = new Mock<IAspect>();
            mockWeapon.Setup( weapon => weapon.Serialize() );

            var item = CreateTestItem();
            item.Stats = mockStats.Object;
            item.Weapon = mockWeapon.Object;

            var data = item.Serialize();

            Assert.IsTrue( data.ContainsKey( "Vnum" ) );
            Assert.IsTrue( data.ContainsKey( "ShortDescr" ) );
            Assert.IsTrue( data.ContainsKey( "Stack" ) );
            Assert.IsTrue( data.ContainsKey( "Stats" ) );

            Assert.IsFalse( data.ContainsKey( "Affects" ) );
            Assert.IsFalse( data.ContainsKey( "Handlers" ) );

            mockStats.VerifyAll();
        }

        [Test]
        public void Deserialize() {
            var original = CreateTestItem();
            original.Stats = StatsTest.CreateTestStats();

            var weapon = AspectFactory.Weapon();
            weapon.BaseDamage = 35;
            original.Weapon = weapon;

            var item = World.Serializer.Deserialize<Item>( SerializeItem( original ) );

            Assert.AreEqual( "ser_item", item.Id );
            Assert.AreEqual( "Serialized Item", item.ShortDescr );
            Assert.IsTrue( item.CanStack );
            Assert.AreEqual( 5, item.Count );
            Assert.AreEqual( 20, item.MaxCount );

            Assert.IsNotNull( item.Stats );
            StatsTest.AssertEqualToTest( item.Stats );

            Assert.IsNotNull( item.Weapon );
            Assert.AreEqual( 35, item.Weapon.BaseDamage );
        }

        [Test]
        public void CreateItem() {
            World.Instance = new World();

            var template1 = new Item { Id = "test_1", ShortDescr = "Test 1" };
            World.Instance.Items.Add( template1.Id, template1 );
            var item1 = Item.Create( "test_1" );
            Assert.AreEqual( 1, item1.Count );
        }

        [Test]
        public void CreateItemWithStack() {
            World.Instance = new World();

            var template2 = new Item { Id = "test_2", ShortDescr = "Test 2", MaxCount = 5 };
            World.Instance.Items.Add( template2.Id, template2 );
            var item2 = Item.Create( "test_2" );
            item2.AddQuantity( 1 );
            Assert.AreEqual( 1, item2.Count );
        }

        [Test]
        public void CountTest() {
            var item = new Item { Id = "test", ShortDescr = "test", MaxCount = 20 };
            item.AddQuantity( 10 );
            Assert.AreEqual( 10, item.Count );
        }

        [Test]
        public void CanStack() {
            var item1 = new Item { Id = "test_1", ShortDescr = "Test 1" };
            Assert.IsFalse( item1.CanStack );

            var item2 = new Item { Id = "test_2", ShortDescr = "Test 2", MaxCount = 20 };
            Assert.IsTrue( item2.CanStack );

            var item3 = new Item { Id = "test_3", ShortDescr = "Test 3", MaxCount = 0 };
            Assert.IsTrue( item3.CanStack );
        }

        [Test]
        public void ItemWithoutStackHasSpaceInStack() {
            var itemWithoutStack = new Item { Id = "test_1", ShortDescr = "item without stack" };
            Assert.IsFalse( itemWithoutStack.HasSpaceInStack );
        }

        [Test]
        public void ItemWithLimitedStackHasSpaceInStack() {
            var itemWithLimitedStack = new Item {
                Id = "test_2",
                ShortDescr = "Item with limited stack",
                MaxCount = 20
            };
            Assert.IsTrue( itemWithLimitedStack.HasSpaceInStack );

            itemWithLimitedStack.AddQuantity( 10 );
            Assert.IsTrue( itemWithLimitedStack.HasSpaceInStack );

            itemWithLimitedStack.AddQuantity( 10 );
            Assert.IsFalse( itemWithLimitedStack.HasSpaceInStack );

            itemWithLimitedStack.AddQuantity( 10 );
            Assert.IsFalse( itemWithLimitedStack.HasSpaceInStack );
        }

        [Test]
        public void ItemWithUnlimitedStackHasSpaceInStack() {
            var itemWithUnlimitedStack = new Item { 
                Id = "test_3", 
                ShortDescr = "Item with unlimited stack",
                MaxCount = 0 };
            Assert.IsTrue( itemWithUnlimitedStack.HasSpaceInStack );

            itemWithUnlimitedStack.AddQuantity( 10 );
            Assert.IsTrue( itemWithUnlimitedStack.HasSpaceInStack );

            itemWithUnlimitedStack.AddQuantity( 10 );
            Assert.IsTrue( itemWithUnlimitedStack.HasSpaceInStack );

            itemWithUnlimitedStack.AddQuantity( 10 );
            Assert.IsTrue( itemWithUnlimitedStack.HasSpaceInStack );
        }


        [Test]
        public void ItemFromTemplate() {
            var mockCloneStats = new Mock<IAspect>();

            var mockStats = new Mock<IAspect>();
            mockStats.Setup( stats => stats.Clone() )
                .Returns( mockCloneStats.Object );

            var mockCloneWeapon = new Mock<IAspect>();

            var mockWeapon = new Mock<IAspect>();
            mockWeapon.Setup( weapon => weapon.Clone() )
                .Returns( mockCloneWeapon.Object );

            var item = CreateTestItem();
            item.Stats = mockStats.Object;
            item.Weapon = mockWeapon.Object;

            var newItem = new Item( item );

            Assert.AreEqual( mockCloneStats.Object, newItem.Stats );
            Assert.AreEqual( mockCloneWeapon.Object, newItem.Weapon );

            mockStats.VerifyAll();
            mockWeapon.VerifyAll();
        }


        [Test]
        public void StatsReceiveItemEvents() {
            var e = Event.Create( "test_event" );

            var mockStats = new Mock<IAspect>();
            mockStats.Setup( stats => stats.ReceiveEvent( e ) );

            var item = CreateTestItem();
            item.Stats = mockStats.Object;

            item.ReceiveEvent( e );

            mockStats.VerifyAll();
        }

        [Test]
        public void EventIsForwardedToWeapon() {
            var e = Event.Create( "test" );

            var mockWeapon = new Mock<IAspect>();
            mockWeapon.Setup( weapon => weapon.ReceiveEvent( e ) );

            var item = CreateTestItem();
            item.Weapon = mockWeapon.Object;

            item.ReceiveEvent( e );

            mockWeapon.VerifyAll();
        }
    }
}
