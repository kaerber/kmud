using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class EventTest : BaseEntityTest {
        [Test]
        public void ChangeToThisTest() {
            var ad = new AffectInfo {
                Handlers = new HandlerSet {
                    { "this_says_text", "import clr\nret_val = 1" }, 
                    { "uncalled handler", "_import clr" }
                },
                Target = AffectTarget.Character
            };

            var ch = new Character();
            ch.Affects.Add( new Affect( ad ) );

            var e = Event.Create( "ch_says_text", EventReturnMethod.None,
                new EventArg( "ch", ch ), new EventArg( "text", "hey ya!" )
            );

            ch.ReceiveEvent( e );
            Assert.AreEqual( 1, e.ReturnValue );
        }

        [Test]
        public void SumReturnMethodSummarizesValues() {
            var e = Event.Create( "sum_test", EventReturnMethod.Sum );
            Assert.AreEqual( 0, e.ReturnValue );

            e.ReturnValue = 100;
            Assert.AreEqual( 100, e.ReturnValue );
        }
    }
}
