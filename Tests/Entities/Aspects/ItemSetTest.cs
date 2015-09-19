using System;
using System.Collections;
using System.Linq;

using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class ItemSetTest : BaseEntityTest {
        [Test]
        public void AddOneItem() {
            var set = new ItemSet();
            var item = new Item { Id = "test", ShortDescr = "test" };
            set.Add( item );
            Assert.AreEqual( 1, set.Count );
        }

        [Test]
        public void AddStack() {
            var set = new ItemSet();
            var item = new Item { Id = "test", ShortDescr = "test", MaxCount = 20 };
            item.AddQuantity( 10 );
            set.Add( item );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 10, set.CountItems( "test" ) );

            var item2 = new Item( item );
            item2.AddQuantity( 7 );
            set.Add( item2 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 17, set.CountItems( "test" ) );

            var item3 = new Item( item );
            item3.AddQuantity( 7 );
            set.Add( item3 );
            Assert.AreEqual( 2, set.Count );
            Assert.AreEqual( 24, set.CountItems( "test" ) );
        }

        [Test]
        public void AddUnlimitedStack() {
            var set = new ItemSet();
            var item = new Item { Id = "test", ShortDescr = "test", MaxCount = 0 };
            item.AddQuantity( 10 );

            set.Add( item );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 10, set.CountItems( "test" ) );

            var item2 = new Item( item );
            item2.AddQuantity( 7 );
            set.Add( item2 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 17, set.CountItems( "test" ) );

            var item3 = new Item( item );
            item3.AddQuantity( 70 );
            set.Add( item3 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 87, set.CountItems( "test" ) );
        }

        [Test]
        public void AddWithItemCount() {
            var set = new ItemSet();
            var itemWithoutStack = new Item { Id = "test_1", ShortDescr = "Test1" };
            set.Add( itemWithoutStack, 1 );
            Assert.AreEqual( 1, set.CountItems( "test_1" ) );

            set.Add( itemWithoutStack, 3 );
            Assert.AreEqual( 4, set.CountItems( "test_1" ) );


            var itemWithoutStack2 = new Item { Id = "test_2", ShortDescr = "Test2" };
            
            set.Add( itemWithoutStack2, 2 );
            Assert.AreEqual( 2, set.CountItems( "test_2" ) );


            var itemWithLimitedStack = new Item { Id = "test_3", ShortDescr = "Test3", MaxCount = 5 };
            itemWithLimitedStack.AddQuantity( 1 );

            set.Add( itemWithLimitedStack, 1 );
            Assert.AreEqual( 1, set.CountItems( "test_3" ) );

            set.Add( itemWithLimitedStack, 1 );
            Assert.AreEqual( 2, set.CountItems( "test_3" ) );
            Assert.AreEqual( 1, set.Count( item => item.Id == "test_3" ) );

            set.Add( itemWithLimitedStack, 4 );
            Assert.AreEqual( 6, set.CountItems( "test_3" ) );
            Assert.AreEqual( 2, set.Count( item => item.Id == "test_3" ) );

            set.Add( itemWithLimitedStack, 100 );
            Assert.AreEqual( 106, set.CountItems( "test_3" ) );
            Assert.AreEqual( 22, set.Count( item => item.Id == "test_3" ) );


            var itemWithUnlimitedStack = new Item { Id = "test_4", ShortDescr = "Test3", MaxCount = 0 };
            itemWithUnlimitedStack.AddQuantity( 1 );

            set.Add( itemWithUnlimitedStack, 1 );
            Assert.AreEqual( 1, set.CountItems( "test_4" ) );

            set.Add( itemWithUnlimitedStack, 200 );
            Assert.AreEqual( 201, set.CountItems( "test_4" ) );
        }


        [Test]
        public void RemoveOneItem() {
            var set = new ItemSet();
            var item = new Item { Id = "test", ShortDescr = "test", MaxCount = 20 };
            item.AddQuantity( 10 );

            set.Add( item );
            Assert.IsTrue( set.Remove( item ) );
            Assert.AreEqual( 0, set.Count );
            Assert.AreEqual( 0, set.CountItems( "test" ) );

            set.Add( item );
            set.Add( item );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 20, set.CountItems( "test" ) );
        }

        [Test]
        public void RemoveItems() {
            var set = new ItemSet();
            var item1 = new Item { Id = "test", ShortDescr = "Test", MaxCount = 20 };
            item1.AddQuantity( 10 );

            set.Add( item1 );


            var item2 = new Item( item1 );
            item2.AddQuantity( 17 );
            set.Add( item2 );

            var removed1 = set.Remove( "test", 1 );
            Assert.AreEqual( 1, removed1.Count );
            Assert.AreEqual( 1, removed1.CountItems( "test" ) );
            Assert.AreEqual( 2, set.Count );
            Assert.AreEqual( 26, set.CountItems( "test" ) );

            var removed2 = set.Remove( "test", 9 );
            Assert.AreEqual( 1, removed2.Count );
            Assert.AreEqual( 9, removed2.CountItems( "test" ) );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 17, set.CountItems( "test" ) );

            var removed3 = set.Remove( "test", 200 );
            Assert.AreEqual( 1, removed3.Count );
            Assert.AreEqual( 17, removed3.CountItems( "test" ) );
            Assert.AreEqual( 0, set.Count );

            set.Add( item1 );

            var removed4 = set.Remove( "test_new", 1 );
            Assert.AreEqual( 1, set.Count );
            Assert.IsNull( removed4 );

            var itemNoStack = new Item { Id = "test_nostack" };
            set.Add( itemNoStack );
            var removed5 = set.Remove( "test_nostack", 100 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( 1, removed5.Count );
            Assert.IsTrue( removed5.Contains( itemNoStack ) );
        }

        [Test]
        public void RemoveItemsUnlimitedStack() {
            var set = new ItemSet();
            var item = new Item { Id = "test", ShortDescr = "test", MaxCount = 0 };
            item.AddQuantity( 10 );
            set.Add( item );

            var item2 = new Item( item );
            item2.AddQuantity( 17 );
            set.Add( item2 );


            var removed1 = set.Remove( "test", 100 );
            Assert.AreEqual( 1, removed1.Count );
            Assert.AreEqual( 27, removed1.CountItems( "test" ) );
            Assert.AreEqual( 0, set.Count );
        }

        [Test]
        public void GetEnumerator() {
            var ienumerable = ( IEnumerable )( new ItemSet() );
            var enumerator = ienumerable.GetEnumerator();
        }

        [Test]
        public void CopyTo() {
            new ItemSet().CopyTo( new Item[10], 0  );
        }

        [Test]
        public void IsReadOnly() {
            Assert.AreEqual( false, new ItemSet().IsReadOnly );
        }

        [Test]
        public void IndexOf() {
            var item = new Item();
            var set = new ItemSet { item };

            Assert.AreEqual( 0, set.IndexOf( item ) );
        }

        [Test]
        public void Insert() {
            var item = new Item();
            var set = new ItemSet();
            set.Insert( 0, item );

            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( item, set[0] );
        }

        [Test]
        public void RemoveAt() {
            var item = new Item();
            var item2 = new Item();
            var set = new ItemSet { item, item2 };

            set.RemoveAt( 1 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( item, set[0] );
        }

        [Test]
        public void Indexer() {
            var item = new Item();
            var item2 = new Item();
            var set = new ItemSet { item, item2 };

            Assert.AreEqual( item2, set[1] );
            Assert.Throws<NotImplementedException>( () => { set[1] = item; } );
        }
    }
}
