using IronPython.Runtime;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class MoneyTest : BaseEntityTest {
        private Item _silver;

        public class CharacterMockUp {
            public ItemSet Inventory = new ItemSet();

            public dynamic Event( string name, EventReturnMethod returnMethod, PythonDictionary parameters ) {
                return null;
            }
        }

        [TestFixtureSetUp]
        public override void FixtureSetup() {
            base.FixtureSetup();

            _silver = new Item {
                Id = "silver", 
                ShortDescr = "silver", 
                Cost = 1, 
                MaxCount = 0, 
                Flags = ItemFlags.Money
            };
        }

        [Test]
        public void EventThisWillPayMoney()
        {
            var aspect = AspectFactory.Money();
            var ev = Event.Create( "this_will_pay_money", EventReturnMethod.Or );
            aspect.ReceiveEvent( ev );
            Assert.IsTrue( ev.ReturnValue );
        }

        [Test]
        public void EbentThisCanPayMoney()
        {
            var aspect = AspectFactory.Money();
            var coin = new Item( _silver );
            coin.AddQuantity( 100 );

            var ch = new CharacterMockUp();
            ch.Inventory.Add( coin );
            aspect.Host = ch;

            var ev = Event.Create( "this_can_pay_money", EventReturnMethod.And, new EventArg( "amount", 80 ) );
            aspect.ReceiveEvent( ev );
            Assert.IsTrue( ev.ReturnValue );

            var cantPay = Event.Create( "this_can_pay_money", EventReturnMethod.And, new EventArg( "amount", 120 ) );
            aspect.ReceiveEvent( cantPay );
            Assert.IsFalse( cantPay.ReturnValue );

        }
    }
}
