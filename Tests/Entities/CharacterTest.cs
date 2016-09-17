using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities.Abilities;

using Moq;

using NUnit.Framework;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Platform.Managers;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class CharacterTest : BaseEntityTest {
        public class CharacterWhiteBox : Character {
            public CharacterWhiteBox() : base() {}

            public void SetActionQueueSet( ActionQueueSet testDouble ) {
                ActionQueueSet = testDouble;
            }
        }

        public interface IMockCombat {
            Character Target { get; }
            void UseAbility( ICombatAbility ability );
            void MakeAttack( IAttack attack );
        }

        [Test]
        public void MobFromTemplate() {
            var template = new Character() {
                Names = "test mob",
                ShortDescr = "Test mob",
                Id = "test_mob"
            };

            template.Stats.Armor = 101;
            template.Stats.Accuracy = 102;
            template.Stats.Health = 103;

            template.NaturalWeapon.BaseDamage = 101;

            var instance = new Character( template );

            Assert.AreEqual( template.Stats.Armor, instance.Stats.Armor );

            Assert.AreEqual( template.Stats.Accuracy, instance.Stats.Accuracy );

            Assert.AreEqual( template.Stats.Health, instance.Stats.Health );

            Assert.AreEqual( template.Names, instance.Names );
            Assert.AreEqual( template.ShortDescr, instance.ShortDescr );
            Assert.AreEqual( template.Id, instance.Id );

            Assert.IsNotNull( instance.NaturalWeapon );
            Assert.AreEqual( template.NaturalWeapon.BaseDamage, instance.NaturalWeapon.BaseDamage );
        }

        [Test]
        public void ReceiveEventTest() {
            var ch = new Character();
            
            ch.ReceiveEvent( Event.Create( "test_event" ) );

            ch.ViewEvent += e => Assert.Pass();
            ch.ReceiveEvent( Event.Create( "test_event" ) );
            Assert.Fail();
        }

       [Test]
        public void KillTest() {
            var ch = new Character();
            var vch = new Character();
            var mockCombat = new Mock<IMockCombat>();
            mockCombat.Setup( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
            ch.Aspects.combat = mockCombat.Object;

            ch.Kill( vch );

            mockCombat.Verify( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
        }

        [Test]
        public void GetFoesInEmptyRoom() {
            var mockCharSet = new Mock<CharacterSet>();

            var mockRoom = new Mock<Room>().Name( "room" );
            mockRoom.Setup( room => room.SelectCharacters( It.IsAny<Predicate<Character>>() ) )
                .Returns( mockCharSet.Object );

            var self = new Character();
            self.SetRoom( mockRoom.Object );

            var foes = self.GetFoes();
            Assert.AreEqual( 0, foes.Count );

            mockRoom.Verify( room => room.SelectCharacters( It.IsAny<Predicate<Character>>() ) );
        }


        [Test]
        public void CharIsSafeFromHimself() {
            var self = new Character();
            Assert.IsTrue( self.IsSafeFrom( self ) );
        }


        [Test]
        public void CharIsNotSafeFromOtherChars() {
            var enemy = new Mock<Character>();

            var self = new Character();

            Assert.IsFalse( self.IsSafeFrom( enemy.Object ) );
        }


        [Test]
        public void TargetTest() {
            var ch = new Character();
            var vch = new Character();
            var mockCombat = new Mock<IMockCombat>();
            mockCombat.Setup( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
            mockCombat.Setup( combat => combat.Target )
                .Returns( vch );

            ch.Aspects.combat = mockCombat.Object;


            ch.Kill( vch );

            Assert.AreEqual( vch, ch.Target );

            mockCombat.Verify( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
            mockCombat.Verify( combat => combat.Target );
        }


        [Test]
        public void InCombatTest() {
            var ch = new Character();
            var vch = new Character();
            var mockCombat = new Mock<IMockCombat>();
            var isInCombat = new Queue<Character>( new[] { null, vch } );
            mockCombat.Setup( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
            mockCombat.Setup( combat => combat.Target )
                .Returns( isInCombat.Dequeue );

            ch.Aspects.combat = mockCombat.Object;

            Assert.IsFalse( ch.IsInCombat );
            ch.Kill( vch );
            Assert.IsTrue( ch.IsInCombat );

            mockCombat.Verify( combat => combat.UseAbility( It.IsAny<KillAbility>() ) );
            mockCombat.Verify( combat => combat.Target );
        }


        [Test]
        public void SendEventTest() {
            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) );

            var ch = new Character();
            ch.SetRoom( mockRoom.Object );

            ch.SendEvent( Event.Create( "test_event" ) );

            mockRoom.VerifyAll();
        }


        [Test]
        public void EventIsForwardedToSpecialization() {
            var eventForwarded = Event.Create( "test_event" );

            var mockSpec = new Mock<IEventTarget>();
            mockSpec.Setup( spec => spec.ReceiveEvent( eventForwarded ) );

            var self = CharacterManager.Create( new Character(), mockSpec.Object );
            self.ReceiveEvent( eventForwarded );

            mockSpec.VerifyAll();
        }

        [Test]
        public void EventIsForwardedToStats() {
            var eventForwarded = Event.Create( "test_event" );

            var mockStats = new Mock<IAspect>();
            mockStats.Setup( spec => spec.ReceiveEvent( eventForwarded ) );

            var self = new Character();
            self.Aspects.stats = mockStats.Object;

            self.ReceiveEvent( eventForwarded );

            mockStats.VerifyAll();
            
        }

        [Test]
        public void EventIsForwardedToEquipment() {
            var e = Event.Create( "test_event" );

            var mockEquipment = new Mock<Equipment>();
            mockEquipment.Setup( equipment => equipment.ReceiveEvent( e ) );

            var self = new Character() { Eq = mockEquipment.Object };

            self.ReceiveEvent( e );

            mockEquipment.VerifyAll();
        }

        [Test]
        public void EnqueueAction() {
            var mockAction = new Mock<CharacterAction>();

            var mockActionQueueSet = new Mock<ActionQueueSet>();
            mockActionQueueSet.Setup( actionQueueSet => actionQueueSet.EnqueueAction( "autoattack", mockAction.Object ) );

            var self = new CharacterWhiteBox();
            self.SetActionQueueSet( mockActionQueueSet.Object );

            self.EnqueueAction( "autoattack", mockAction.Object );

            mockActionQueueSet.VerifyAll();
        }

        [Test]
        public void AttackIsForwardedToCombat() {
            var eArg = Event.Create( "null" );

            var mockRoom = new Mock<Room>();
            mockRoom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => { eArg = e; } );

            var mockEnemy = new Mock<Character>();

            var mockAttack = new Mock<IAttack>();

            var mockCombat = new Mock<IMockCombat>();
            mockCombat.Setup( combat => combat.MakeAttack( mockAttack.Object ) );
            mockCombat.Setup( combat => combat.Target )
                .Returns( mockEnemy.Object );

            var self = new Character();
            self.SetRoom( mockRoom.Object );
            self.Aspects.combat = mockCombat.Object;

            self.MakeAttack( mockAttack.Object );

            mockCombat.VerifyAll();
            mockRoom.VerifyAll();

            Assert.AreEqual( "ch_is_attacking_ch1", eArg.Name );
        }

        [Test]
        public void SetRoomRemovesCharFromPreviousRoomAndAddsToNext() {
            var ch = new Character()
                { ShortDescr = "Character", Names = "test character" };

            var mockRoomPrev = new Mock<Room>();
            mockRoomPrev.Setup( room => room.RemoveCharacter( ch ) );

            ch.SetRoom( mockRoomPrev.Object );

            var mockRoomNext = new Mock<Room>();
            mockRoomNext.Setup( room => room.AddCharacter( ch ) );

            ch.SetRoom( mockRoomNext.Object );

            mockRoomPrev.VerifyAll();
            mockRoomNext.VerifyAll();
        }

        [Test]
        public void SpecIsNotNullTest() {
            var ch = new Character();
            ch.Initialize();
            Assert.IsNotNull( ch.Spec );
        }

        [Test]
        public void ChLeftRoomIsFiredInTheRoomCharLeft() {
            var events = new Dictionary<string, Event>();
            var mockRoomFrom = new Mock<Room>();
            mockRoomFrom.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => events.Add( e.Name, e ) );

            var exit = new Mock<Exit>();
            var ch = new Character();
            ch.SetRoom( mockRoomFrom.Object );

            ch.MoveToRoom( exit.Object );

            mockRoomFrom.VerifyAll();
            Assert.IsTrue( events.ContainsKey( "ch_can_leave_room" ) );
            Assert.IsTrue( events.ContainsKey( "ch_left_room" ) );
        }

        [Test]
        public void ChEnteredRoomIsFiredInTheRoomCharEntered() {
            var events = new Dictionary<string, Event>();
            var mockRoomFrom = new Mock<Room>();

            var mockRoomTo = new Mock<Room>();
            mockRoomTo.Setup( room => room.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => events.Add( e.Name, e ) );
            var exit = new Mock<Exit>();
            exit.Setup( e => e.To ).Returns( mockRoomTo.Object );

            var ch = new Character();
            ch.SetRoom( mockRoomFrom.Object );

            ch.MoveToRoom( exit.Object );

            mockRoomTo.VerifyAll();
            Assert.IsTrue( events.ContainsKey( "ch_can_enter_room" ) );
            Assert.IsTrue( events.ContainsKey( "ch_entered_room" ) );
        }
    }
}
