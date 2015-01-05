using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class AffectSetTest : BaseEntityTest {
        private AffectInfo _hordeMode;
        private AffectInfo _sanctuary;
        private AffectInfo _firewall;

        [TestFixtureSetUp]
        public void SetUp() {
            World.Instance = new World();

            _hordeMode = new AffectInfo {
                Name = "horde_mode", 
                Target = AffectTarget.Character, 
                Flags = AffectFlags.NoDeath,
                Handlers = new HandlerSet()
            };
            World.Instance.Affects.Add( _hordeMode.Name, _hordeMode );

            _sanctuary = new AffectInfo {
                Name = "sanctuary", 
                Target = AffectTarget.Character, 
                Flags = AffectFlags.Multiple,
                Handlers = new HandlerSet()
            };
            World.Instance.Affects.Add( _sanctuary.Name, _sanctuary );

            _firewall = new AffectInfo { Name = "firewall", Target = AffectTarget.Room };
            World.Instance.Affects.Add( _firewall.Name, _firewall );
        }

        [Test]
        public void ConstructorTest() {
            var list = new List<Affect> { new Affect( _hordeMode ), new Affect( _sanctuary ) };

            var setFromList = new AffectSet( list, null );
            Assert.AreEqual( 2, setFromList.Count );
            Assert.AreEqual( list[0], setFromList[0] );
            Assert.AreEqual( list[1], setFromList[1] );
        }

        [Test]
        public void ItemTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            Assert.AreEqual( horde, set[0] );
            Assert.Throws<NotSupportedException>( () => set[0] = sanc );
        }

        [Test]
        public void CountTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );

            Assert.AreEqual( 2, set.Count );
        }

        [Test]
        public void AddTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );
            var fwall = new Affect( _firewall );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );
            Assert.Throws<EntityException>( () => set.Add( fwall ) );

            Assert.AreEqual( 2, set.Count );
            Assert.AreEqual( horde, set[0] );
            Assert.AreEqual( sanc, set[1] );
        }

        [Test]
        public void InsertTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );
            var fwall = new Affect( _firewall );

            var affectSet = new AffectSet();
            affectSet.SetHost( new Character() );

            affectSet.Insert( 0, horde );
            affectSet.Insert( 0, sanc );
            Assert.Throws<EntityException>( () => affectSet.Insert( 0, fwall ) );

            Assert.AreEqual( horde, affectSet[1] );
            Assert.AreEqual( sanc, affectSet[0] );
        }

        [Test]
        public void CastTest() {
            var affectSet = new AffectSet();
            affectSet.SetHost( new Character() );
            var affect = affectSet.Cast( "sanctuary", World.TimeHour );

            Assert.AreEqual( affect, affectSet[0] );
            Assert.AreEqual( "sanctuary", affect.Name );
            Assert.AreEqual( AffectTarget.Character, affect.Target );
            Assert.AreEqual( World.TimeHour, affect.Duration );

            var affectSanc2 = affectSet.Cast( "sanctuary", World.TimeHour );
            Assert.AreEqual( affectSanc2, affectSet[ 1 ] );
            Assert.AreEqual( "sanctuary", affectSanc2.Name );
            Assert.AreEqual( AffectTarget.Character, affectSanc2.Target );
            Assert.AreEqual( World.TimeHour, affectSanc2.Duration );

            var affectHorde = affectSet.Cast( "horde_mode", World.TimeHour );
            Assert.AreEqual( 3, affectSet.Count );
            Assert.AreEqual( affectHorde, affectSet[ 2 ] );
            Assert.AreEqual( "horde_mode", affectHorde.Name );

            var affectHorde2 = affectSet.Cast( "horde_mode", World.TimeHour );
            Assert.AreEqual( 3, affectSet.Count );
            Assert.AreEqual( null, affectHorde2 );
        }

        [Test]
        public void ClearAffectTest() {
            var affectSet = new AffectSet();
            affectSet.SetHost( new Character() );
            affectSet.Cast( "sanctuary", World.TimeHour );
            affectSet.Cast( "sanctuary", World.TimeHour );
            affectSet.Cast( "horde_mode", World.TimeHour );
            affectSet.Cast( "horde_mode", World.TimeHour );
         
            Assert.AreEqual( 3, affectSet.Count );

            affectSet.Clear( "sanctuary" );
            Assert.AreEqual( 1, affectSet.Count );
            Assert.IsFalse( affectSet.Contains( "sanctuary" ) );

            affectSet.Cast( "sanctuary", World.TimeHour );
            affectSet.Clear( "horde_mode" );
            Assert.AreEqual( 1, affectSet.Count );
            Assert.IsFalse( affectSet.Contains( "horde_mode" ) );
        }

        [Test]
        public void RemoveTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );
            set.Remove( horde );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( sanc, set[0] );
        }

        [Test]
        public void RemoveAtTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );
            set.RemoveAt( 0 );
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( sanc, set[0] );
        }

        [Test]
        public void ClearTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );
            set.Add( horde );
            set.Add( sanc );
            Assert.AreEqual( 2, set.Count );

            set.Clear();
            Assert.AreEqual( 0, set.Count );
        }

        [Test]
        public void ContainsTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );

            Assert.IsTrue( set.Contains( horde ) );
            Assert.IsFalse( set.Contains( sanc ) );
            Assert.IsTrue( set.Contains( _hordeMode.Name )  );
            Assert.IsFalse( set.Contains( _sanctuary.Name ) );
        }

        [Test]
        public void IndexOfTest() {
            var horde = new Affect( _hordeMode );

            var affectSet = new AffectSet();
            affectSet.SetHost( new Character() );

            affectSet.Add( horde );

            Assert.AreEqual( 1, affectSet.Count );
            Assert.AreEqual( 0, affectSet.IndexOf( horde ) );
            Assert.IsNotNull( _hordeMode );
            Assert.IsNotNull( affectSet[0] );
            Assert.IsNotNull( _hordeMode.Name );
            Assert.IsNotNull( affectSet[0].Name );
            Assert.AreEqual( _hordeMode.Name, affectSet[0].Name );
        }

        [Test]
        public void CopyToTest() {
            var horde = new Affect( _hordeMode );
            var sanc = new Affect( _sanctuary );

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );

            var array = new Affect[5];
            set.CopyTo( array, 2 );
            Assert.AreEqual( horde, array[2] );
            Assert.AreEqual( sanc, array[3] );
        }

        [Test]
        public void IsReadOnlyTest() {
            var set = new AffectSet();
            Assert.IsFalse( set.IsReadOnly );
        }

        [Test]
        public void GetEnumeratorTest() {
            IEnumerable set = new AffectSet();
            foreach( var affect in set )
                Assert.Fail();
        }

        [Test]
        public void UpdateTest() {
            World.Instance = new World { Time = 0 };

            var horde = new Affect( _hordeMode ) { Duration = 10 };
            var sanc = new Affect( _sanctuary ) { Duration = 20 };

            var set = new AffectSet();
            set.SetHost( new Character() );

            set.Add( horde );
            set.Add( sanc );

            set.Update();
            Assert.AreEqual( 2, set.Count );

            World.Instance.Time = 15;
            set.Update();
            Assert.AreEqual( 1, set.Count );
            Assert.AreEqual( sanc, set[0] );

            World.Instance.Time = 25;
            set.Update();
            Assert.AreEqual( 0, set.Count );
        }

        [Test]
        public void SetHostTest() {
            var set = new AffectSet();
            var ch = new Character();
            set.SetHost( ch );

            var field = typeof( AffectSet )
                .GetField( "_host",
                    BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public );
            Assert.NotNull( field );

            Assert.AreEqual( ch, field.GetValue( set ) );
        }

        [Test]
        public void ReceiveEventTest() {
            var data = new AffectInfo {
                Target = AffectTarget.Character,
                Handlers = new HandlerSet {
                    { "test_event", "10/0" },
                    { "test_event_correct", "10/1" }
                }
            };

            var ch = new Character();
            ch.Affects.Add( new Affect( data ) );

            Assert.Throws<DivideByZeroException>( () => ch.Affects.ReceiveEvent( Event.Create( "test_event" ) ) );

            ch.Affects.ReceiveEvent( Event.Create( "test_event_correct" ) );
        }

        [Test]
        public void CheckAffectEligibilityTest() {
            var method = typeof( AffectSet )
                .GetMethod( "CheckAffectEligibility",
                    BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public );

            var set = new AffectSet();
            var data = new AffectInfo();
            var affect = new Affect( data );

            var room = new Room();
            var item = new Item();
            var ch = new Character();

            set.SetHost( room );

            data.Target = AffectTarget.Room;
            Assert.AreEqual( true, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Item;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Character;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );


            set.SetHost( item );

            data.Target = AffectTarget.Room;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Item;
            Assert.AreEqual( true, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Character;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );


            set.SetHost( ch );

            data.Target = AffectTarget.Room;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Item;
            Assert.AreEqual( false, method.Invoke( set, new object[] { affect } ) );

            data.Target = AffectTarget.Character;
            Assert.AreEqual( true, method.Invoke( set, new object[] { affect } ) );
        }
    }
}
