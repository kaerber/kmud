using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities.Abilities {
    [TestFixture]
    public class KillAbilityTest : BaseEntityTest {
        [Test]
        public void SelectFirstFoeWhenNotFighting() {
            var self = new Mock<Character>();
            var foe1 = new Mock<Character>();
            var foes = new Mock<CharacterSet>();

            self.Setup( s => s.GetFoes() )
                .Returns( foes.Object );
            foes.Setup( f => f[0] )
                .Returns( foe1.Object );
            foes.SetupGet( f => f.Count )
                .Returns( 3 );


            var ability = new KillAbility( self.Object, null );
            Assert.AreEqual( foe1.Object,
                ability.SelectMainTarget( currentTarget: null ) );

            self.Verify( s => s.GetFoes(), Times.Once() );
            foes.Verify( s => s[0], Times.Once() );
        }


        [Test]
        public void SelectNooneWhenInEmptyRoom() {
            var mockSelf = new Mock<Character>();
            var mockFoes = new Mock<CharacterSet>();

            mockSelf.Setup( s => s.GetFoes() )
                .Returns( mockFoes.Object );

            var ability = new KillAbility( mockSelf.Object, null );
            Assert.IsNull( ability.SelectMainTarget( currentTarget: null ) );

            mockSelf.Verify( self => self.GetFoes(), Times.Once() );
            mockFoes.Verify( foes => foes[0], Times.Never() );
        }


        [Test]
        public void SelectTargetWhenNotFighting() {
            var mockNotTarget = new Mock<Character>();

            var mockTarget = new Mock<Character>();

            var mockFoes = new Mock<CharacterSet>();
            mockFoes.Setup( foes => foes[0] )
                .Returns( mockNotTarget.Object );
            mockFoes.Setup( foes => foes[1] )
                .Returns( mockTarget.Object );
            mockFoes.Setup( foes => foes.Count )
                .Returns( 2 );

            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.GetFoes() )
                .Returns( mockFoes.Object );


            var ability = new KillAbility( self: mockSelf.Object, target: mockTarget.Object );

            
            Assert.AreEqual( mockTarget.Object, ability.SelectMainTarget( currentTarget: null ) );

            mockSelf.Verify( self => self.GetFoes(), Times.Never() );
        }


        [Test]
        public void SelectTargetWhenFighting() {
            var mockCurrentTarget = new Mock<Character>();

            var mockTarget = new Mock<Character>();

            var mockFoes = new Mock<CharacterSet>();
            mockFoes.Setup( foes => foes[0] )
                .Returns( mockCurrentTarget.Object );
            mockFoes.Setup( foes => foes[1] )
                .Returns( mockTarget.Object );
            mockFoes.Setup( foes => foes.Count )
                .Returns( 2 );

            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.GetFoes() )
                .Returns( mockFoes.Object );


            var ability = new KillAbility( self: mockSelf.Object, target: mockTarget.Object );

            
            Assert.AreEqual( mockTarget.Object, ability.SelectMainTarget( currentTarget: mockCurrentTarget.Object ) );

            mockSelf.Verify( self => self.GetFoes(), Times.Never() );
        }

        [Test]
        public void CycleForwardWhenInFightAndNoTarget() {
            var mockCurrentTarget = new Mock<Character>().Name( "current target" );
            var mockNextTarget = new Mock<Character>().Name( "next target" );

            var mockFoes = new Mock<CharacterSet>();
            mockFoes.Setup( foes => foes[0] )
                .Returns( mockCurrentTarget.Object );
            mockFoes.Setup( foes => foes[1] )
                .Returns( mockNextTarget.Object );
            mockFoes.Setup( foes => foes.Count )
                .Returns( 2 );
            mockFoes.Setup( foes => foes.IndexOf( mockCurrentTarget.Object ) )
                .Returns( 0 );
            
            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.GetFoes() )
                .Returns( mockFoes.Object );

            var ability = new KillAbility( mockSelf.Object, null );
            Assert.AreEqual( mockNextTarget.Object, ability.SelectMainTarget( currentTarget: mockCurrentTarget.Object ) );

            mockSelf.Verify( self => self.GetFoes(), Times.Once() );
        }

        [Test]
        public void CycleToStartWhenLastFoeIsCurrentAndNoTarget() {
            var mockCurrentTarget = new Mock<Character>().Name( "current target" );
            var mockNextTarget = new Mock<Character>().Name( "next target" );

            var mockFoes = new Mock<CharacterSet>();
            mockFoes.Setup( foes => foes[0] )
                .Returns( mockNextTarget.Object );
            mockFoes.Setup( foes => foes[1] )
                .Returns( mockCurrentTarget.Object );
            mockFoes.Setup( foes => foes.Count )
                .Returns( 2 );
            mockFoes.Setup( foes => foes.IndexOf( mockCurrentTarget.Object ) )
                .Returns( 1 );
            
            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.GetFoes() )
                .Returns( mockFoes.Object );

            var ability = new KillAbility( mockSelf.Object, null );
            Assert.AreEqual( mockNextTarget.Object, 
                             ability.SelectMainTarget( currentTarget: mockCurrentTarget.Object ) );

            mockSelf.Verify( self => self.GetFoes(), Times.Once() );
        }

        [Test]
        public void ActivateDoesNothing() {
            var mockSelf = new Mock<Character>();

            var ability = new KillAbility( mockSelf.Object, null );
            ability.Activate();
        }
    }
}
