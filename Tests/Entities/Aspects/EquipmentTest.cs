using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class EquipmentTest : BaseEntityTest {
        [Test]
        public void EventIsForwardedToItems() {
            var e = Event.Create( "test_item" );

            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.ReceiveEvent( e ) );
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var equipment = new Equipment();

            equipment.Equip( mockItem.Object );

            equipment.ReceiveEvent( e );

            mockItem.Verify( item => item.ReceiveEvent( e ) );
        }


        [Test]
        public void EquippedItemIsAddedToEquipment() {
            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.Body );

            var equipment = new Equipment();

            equipment.Equip( mockItem.Object );

            Assert.IsTrue( equipment.Have( mockItem.Object ) );
        }


        [Test]
        public void ItemIsAddedToItsSlot() {
            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.RightHand );

            var equipment = new Equipment();

            equipment.Equip( mockItem.Object );

            Assert.AreEqual( mockItem.Object, equipment.Get( WearLocation.RightHand ) );
        }


        [Test]
        public void OtherSlotsAreUntouchedByEquippingAnItem() {
            var mockItem = new Mock<Item>();
            mockItem.Setup( item => item.WearLoc )
                .Returns( WearLocation.RightHand );

            var mockArmor = new Mock<Item>();
            mockArmor.Setup( armor => armor.WearLoc )
                .Returns( WearLocation.Body );

            var equipment = new Equipment();

            equipment.Equip( mockArmor.Object );
            Assert.IsTrue( equipment.Have( WearLocation.Body ) );
            Assert.IsFalse( equipment.Have( WearLocation.RightHand ) );
            Assert.IsFalse( equipment.Have( WearLocation.RightRing ) );

            equipment.Equip( mockItem.Object );
            Assert.IsTrue( equipment.Have( WearLocation.Body ) );
            Assert.IsTrue( equipment.Have( WearLocation.RightHand ) );
            Assert.IsFalse( equipment.Have( WearLocation.RightRing ) );
        }
    }
}
