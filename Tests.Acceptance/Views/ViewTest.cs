using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Views
{
    [TestFixture]
    public class ViewTest : BaseAcceptanceTest
    {
        private Character AddChar( string name )
        {
            var ch = new Character { ShortDescr = name, Names = name };
            ch.SetRoom( Room );
            Room.Characters.Add( ch );

            return ch;
        }

        [SetUp]
        public void Setup()
        {
            CreateTestEnvironment();
        }


        [TestCase( "see_condition" )]
        [TestCase( "this_can_recall" )]
        [TestCase( "this_recalled" )]
        [TestCase( "this_has_quit" )]
        public void TestBasicThisEvent( string ename )
        {
            TestModelEvent( ename, new EventArg( "this", Model ) );
        }

        [TestCase( "ch_died" )]
        [TestCase( "ch_recalled_out" )]
        [TestCase( "ch_recalled_in" )]
        [TestCase( "ch_has_quit" )]
        public void TestBasicChEvent( string ename )
        {
            var ch = AddChar( "ch" );
            TestModelEvent( ename, new EventArg( "ch", ch ) );
        }


        [Test]
        public void ThisSawSomething()
        {
            TestModelEvent( "this_saw_something", 
                new EventArg( "message", "test" ) );
        }

        [Test]
        public void ThisStoppedFightingCh1()
        {
            var ch1 = AddChar( "ch1" );
            TestModelEvent( "this_stopped_fighting_ch1", 
                new EventArg( "this", Model ), new EventArg( "ch1", ch1 ) );
        }

        [Test]
        public void ChMissedCh1()
        {
            var ch = AddChar( "ch" );
            var ch1 = AddChar( "ch1" );

            TestModelEvent( "ch_missed_ch1", 
                new EventArg( "ch", ch ), new EventArg( "ch1", ch1 ) );
        }

        [Test]
        public void ChTookDamageFromCh1()
        {
            var ch = AddChar( "ch" );
            var ch1 = AddChar( "ch1" );

            TestModelEvent( "ch_took_damage_from_ch1", 
                new EventArg( "ch", ch ), 
                new EventArg( "ch1", ch1 ), 
                new EventArg( "damage", 100 ) );
        }

        [Test]
        public void ChWentFromRoomToRoom()
        {
            var ch = AddChar( "ch" );

            var room2 = new Room { Id = "test2", ShortDescr = "Test2" };
            Room.Exits.Add( new Exit { Name = "west", To = room2 } );
            room2.Exits.Add( new Exit { Name = "east", To = Room } );

            TestModelEvent( "ch_went_from_room_to_room", 
                new EventArg( "ch", ch ),
                new EventArg( "room_from", Room ), new EventArg( "room_to", room2 ),
                new EventArg( "exit", Room.Exits[room2] ), 
                new EventArg( "entrance", room2.Exits[Room] ) );
        }

        [Test]
        public void ChGotItem()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };

            TestModelEvent( "ch_got_item", 
                new EventArg( "ch", ch ), new EventArg( "item", item ) );
        }

        [Test]
        public void ChGotItemFromContainer()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };
            var container = new Item { ShortDescr = "container", Names = "container" };

            TestModelEvent( "ch_got_item_from_container", 
                new EventArg( "ch", ch ),
                new EventArg( "item", item ),
                new EventArg( "container", container ) );
        }

        [Test]
        public void ChRemovedItem()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };

            TestModelEvent( "ch_removed_item", 
                new EventArg( "ch", ch ), new EventArg( "item", item ) );
        }

        [Test]
        public void ChEquippedItem()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };

            TestModelEvent( "ch_equipped_item", 
                new EventArg( "ch", ch ), new EventArg( "item", item ) );
        }

        [Test]
        public void ChDroppedItem()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };

            TestModelEvent( "ch_dropped_item", 
                new EventArg( "ch", ch ), new EventArg( "item", item ) );
        }

        [Test]
        public void ChPutItemIntoContainer()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };
            var container = new Item { ShortDescr = "container", Names = "container" };

            TestModelEvent( "ch_put_item_into_container", 
                new EventArg( "ch", ch ),
                new EventArg( "item", item ), new EventArg( "container", container ) );
        }

        [Test]
        public void ChBoughtItem()
        {
            var ch = AddChar( "ch" );
            var item = new Item { ShortDescr = "item", Names = "item" };
            var money = new ItemSet
            {
                new Item { ShortDescr = "gold", Names = "gold", Cost = 100 },
                new Item { ShortDescr = "silver", Names = "silver", Cost = 1 }
            };
            money.Find( "gold" ).AddQuantity( 9 );
            money.Find( "silver" ).AddQuantity( 27 );

            var change = new ItemSet
            {
                new Item { ShortDescr = "silver", Names = "silver", Cost = 1 }
            };
            change.Find( "silver" ).AddQuantity( 8 );

            TestModelEvent( "ch_bought_item", 
                new EventArg( "ch", ch ),
                new EventArg( "item", item ),
                new EventArg( "money", money ),
                new EventArg( "change", change ) );
        }

        [Test]
        public void ChSaidText()
        {
            var ch = AddChar( "ch" );
            TestModelEvent( "ch_said_text",
                new EventArg( "ch", ch ), new EventArg( "text", "test" ) );
        }
    }
}
