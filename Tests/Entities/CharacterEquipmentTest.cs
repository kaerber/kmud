using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Server;
using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class CharacterEquipmentTest : BaseEntityTest {
        [SetUp]
        public void SetUp() {
            UnityConfigurator.Configure();
        }

        [Test]
        public void EquipTest() {
            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockInventory = new Mock<ItemSet>();
            mockInventory.Setup( inventory => inventory.Contains( mockItem.Object ) )
                .Returns( true );
            var mockRoom = new Mock<Room>();

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Equip( mockItem.Object ) );

            var self = new Character()
                { Eq = mockEquipment.Object, Inventory = mockInventory.Object };
            self.SetRoom( mockRoom.Object );

            self.Equip( mockItem.Object );

            mockEquipment.VerifyAll();
        }


        [Test]
        public void EquippedItemIsRemovedFromInventory() {
            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockInventory = new Mock<ItemSet>();
            mockInventory.Setup( inv => inv.Contains( mockItem.Object ) )
                .Returns( true );
            mockInventory.Setup( inventory => inventory.Remove( mockItem.Object ) )
                .Returns( true );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Equip( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) );

            var self = new Character()
                { Eq = mockEquipment.Object, Inventory = mockInventory.Object };
            self.SetRoom( mockRoom.Object );

            self.Equip( mockItem.Object );

            mockInventory.Verify( inventory => inventory.Remove( mockItem.Object ) );
            mockEquipment.VerifyAll();
        }


        [Test]
        public void CanEventIsFiredOnEquip() {
            var listEvents = new List<Event>();

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockInventory = new Mock<ItemSet>();
            mockInventory.Setup( inventory => inventory.Contains( mockItem.Object ) )
                .Returns( true );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Equip( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( listEvents.Add );

            var self = new Character()
                { Inventory = mockInventory.Object, Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Equip( mockItem.Object );

            mockRoom.VerifyAll();

            Assert.AreEqual( "ch_can_equip_item", listEvents[0].Name );
            Assert.AreEqual( "ch_equipped_item", listEvents[1].Name );
        }


        [Test]
        public void CanNotEventForbidsEquip() {
            var listEvents = new List<Event>();

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockInventory = new Mock<ItemSet>();
            mockInventory.Setup( inventory => inventory.Contains( mockItem.Object ) )
                .Returns( true );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Equip( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( ev => {
                    listEvents.Add( ev );
                    if( ev.Name == "ch_can_equip_item" )
                        ev.ReturnValue = false;
                } );

            var self = new Character()
                { Inventory = mockInventory.Object, Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Equip( mockItem.Object );

            mockRoom.VerifyAll();
            mockEquipment.Verify( equipment => equipment.Equip( mockItem.Object ), Times.Never() );

            Assert.AreEqual( 1, listEvents.Count );
            Assert.AreEqual( "ch_can_equip_item", listEvents[0].Name );
        }

        [Test]
        public void CanEventIsFiredWhenUnequipping() {
            var eventList = new List<Event>();

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Have( mockItem.Object ) )
                .Returns( true );
            mockEquipment.Setup( equipment => equipment.Remove( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( eventList.Add );

            var self = new Character()
                { Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Unequip( mockItem.Object );

            Assert.GreaterOrEqual( eventList.Count, 1 );
            Assert.AreEqual( "ch_can_remove_item", eventList[0].Name );

            mockEquipment.Verify( equipment => equipment.Remove( mockItem.Object ), Times.AtLeastOnce() );
        }

        [Test]
        public void CanEventDeniesRemovingItem() {
            var eventList = new List<Event>();

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Have( mockItem.Object ) )
                .Returns( true );
            mockEquipment.Setup( equipment => equipment.Remove( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => { 
                    eventList.Add( e );
                    if( e.Name == "ch_can_remove_item" )
                        e.ReturnValue = false;
                } );

            var self = new Character()
                { Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Unequip( mockItem.Object );

            Assert.AreEqual( eventList.Count, 1 );
            Assert.AreEqual( "ch_can_remove_item", eventList[0].Name );

            mockEquipment.Verify( equipment => equipment.Remove( mockItem.Object ), Times.Never() );
        }


        [Test]
        public void DidEventIsFiredAfterRemovingItem() {
            var eventList = new List<Event>();

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Have( mockItem.Object ) )
                .Returns( true );
            mockEquipment.Setup( equipment => equipment.Remove( mockItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => { 
                    eventList.Add( e );
                    if( e.Name == "ch_can_remove_item" )
                        e.ReturnValue = true;
                } );

            var self = new Character()
                { Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Unequip( mockItem.Object );

            Assert.AreEqual( eventList.Count, 2 );
            Assert.AreEqual( "ch_can_remove_item", eventList[0].Name );
            Assert.AreEqual( "ch_removed_item", eventList[1].Name );

            mockEquipment.Verify( equipment => equipment.Remove( mockItem.Object ), Times.Once() );
        }


        [Test]
        public void CharReplacesOneItemWithAnother() {
            var eventList = new List<Event>();

            var mockNewItem = new Mock<Item>();
            mockNewItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockOldItem = new Mock<Item>();
            mockOldItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var mockInventory = new Mock<ItemSet>();
            mockInventory.Setup( inventory => inventory.Contains( mockNewItem.Object ) )
                .Returns( true );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.Have( WearLocation.Body ) )
                .Returns( true );
            mockEquipment.Setup( equipment => equipment.Have( mockOldItem.Object ) )
                .Returns( true );
            mockEquipment.Setup( equipment => equipment.Get( WearLocation.Body ) )
                .Returns( mockOldItem.Object );
            mockEquipment.Setup( equipment => equipment.Remove( mockOldItem.Object ) );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( ev =>
                {
                    if( ev.Name == "ch_can_equip_item" || ev.Name == "ch_can_remove_item" )
                        ev.ReturnValue = true;
                    eventList.Add( ev );
                } );

            var self = new Character()
                { Inventory = mockInventory.Object, Eq = mockEquipment.Object };
            self.SetRoom( mockRoom.Object );

            self.Equip( mockNewItem.Object );

            mockEquipment.Verify( equipment => equipment.Have( WearLocation.Body ) );
            mockEquipment.Verify( equipment => equipment.Get( WearLocation.Body ) );
            mockEquipment.Verify( equipment => equipment.Remove( mockOldItem.Object ) );
            Assert.AreEqual( "ch_can_equip_item", eventList[0].Name );
            Assert.AreEqual( "ch_can_remove_item", eventList[1].Name );
            Assert.AreEqual( "ch_removed_item", eventList[2].Name );
            Assert.AreEqual( "ch_equipped_item", eventList[3].Name );
        }
    }
}
