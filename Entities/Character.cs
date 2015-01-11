using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using IronPython.Runtime;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities
{
    [Flags]
    public enum MobFlags {
        Sentinel = 1,
        StayArea = 2
    }

    public class Character : Entity {
        public Character() {
            NaturalWeapon = AspectFactory.Weapon();
            NaturalWeapon.BaseDamage = 1;

            Setup();
        }

        public Character( Character template ) : base( template.Id, template.Names, template.ShortDescr ) {
            NaturalWeapon = template.NaturalWeapon.Clone();
            Flags = template.Flags;
            Setup();
            if( template.Aspects != null )
                Aspects = template.Aspects.Clone();
        }

        private void Setup() {
            Race = RaceFactory.Default;
            Spec = SpecFactory.Warrior;

            Inventory = new ItemSet();
            Eq = new Equipment();
            Skills = new SkillSet { Host = this };
            Data = new Dictionary<string, string>();

            ActionQueueSet = new ActionQueueSet( this );
            UpdateQueue = new TimedEventQueue( null );

            Aspects.stats = AspectFactory.Stats();
            Aspects.health = AspectFactory.Health();
            Aspects.combat = AspectFactory.Combat();
            Aspects.movement = AspectFactory.Movement();
        }


        public IEventHandler Race { get; set; }
        public IEventHandler Spec;

        public bool IsInCombat { get { return Target != null; } }
        public virtual Character Target { get { return Combat.Target; } }


        private Room _room;

        private Equipment _eq;

        public Room RespawnAt;

        public ItemSet Inventory;

        public SkillSet Skills;
        public Dictionary<string, string> Data;

        public Action<Event> ViewEvent;


        public Room Room { get { return ( _room ); } }

        public Equipment Eq {
            get { return ( _eq ); }
            set {
                _eq = value;
                _eq.Host = this;
            }
        }

        [MudEdit( "Natural weapons (punches, claws, bites, etc)", CustomType = "PythonObject" )]
        public dynamic NaturalWeapon { get; set; }

        [MudEdit( "Flags" )]
        public MobFlags Flags { get; set; }

        [Obsolete]
        public dynamic Health { get { return Aspects.health; } }

        [Obsolete]
        public dynamic Movement { get { return Aspects.movement; } }

        private dynamic Combat { get { return Aspects.combat; } }

        [MudEdit( "Stats", CustomType = "PythonObject" )]
        public dynamic Stats { get { return Aspects.stats; } }

        public ActionQueueSet ActionQueueSet { get; set; }

        #region Save & Load
        public override IDictionary<string, object> Serialize() {
            var data = base.Serialize()
                .AddEx( "Flags", Flags )
                .AddIf( "Inventory", Inventory, Inventory.Count > 0 )
                .AddIf( "Equipment", Eq, Eq.Count > 0 )

                .AddIf( "LoginAt", Room != null ? Room.Id : string.Empty, Room != null )
                .AddIf( "RespawnAt", RespawnAt != null ? RespawnAt.Id : string.Empty, RespawnAt != null )

                .AddIf( "Skills", Skills, Skills != null && Skills.Count > 0 )

                .AddIf( "Data", Data, Data != null && Data.Keys.Count > 0 );

            data.Add( "Stats", Stats.Serialize() );
            data.Add( "NaturalWeapon", NaturalWeapon.Serialize() );

            return data;
        }

        public override ISerialized Deserialize( IDictionary<string, object> data ) {
            Contract.Requires( World != null );

            base.Deserialize( data );

            if( data.ContainsKey( "Stats" ) )
                Stats.Deserialize( data["Stats"] );

            if( data.ContainsKey( "NaturalWeapon" ) )
                NaturalWeapon.Deserialize( data["NaturalWeapon"] );

            Flags = World.ConvertToTypeExs<MobFlags>( data, "Flags" );

            Inventory = new ItemSet( 
                World.ConvertToTypeEx( data, "Inventory", new List<Item>() ) );
            Eq = World.ConvertToTypeEx( data, "Equipment", new Equipment() );

            var respawnAt = World.ConvertToTypeEx<string>( data, "RespawnAt" );
            RespawnAt = World.GetRoom( respawnAt );

            var loginAt = World.ConvertToTypeEx<string>( data, "LoginAt" );
            SetRoom( World.GetRoom( loginAt ) );
            if( _room == null )
                SetRoom( RespawnAt );


            Skills = World.ConvertToTypeEx( data, "Skills", new SkillSet() );
            Skills.Host = this;

            Data = World.ConvertToTypeEx( data, "Data", new Dictionary<string, string>() );

            return ( this );
        }
        #endregion

        public override string ToString() { return( ShortDescr ); }

        public void Restore() {
            Health.Restore();
        }


        public void Die() {
            Contract.Requires( Room != null );

            if( !this.CanDo( "die" ) )
                return;
            
            var corpse = CreateCorpse();
            corpse.Container.Items.AddRange( Inventory );
            Inventory.Clear();
            corpse.Container.Items.AddRange( Eq.Items );
            Eq.Clear();

            this.Did( "died" );
            SetRoom( RespawnAt );
            if( RespawnAt != null )
                Restore();
        }


        protected Item CreateCorpse() {
            var corpse = Room.Items.Load( "ch_corpse" );
            corpse.Names = string.Format( corpse.Names, Names );
            corpse.ShortDescr = string.Format( corpse.ShortDescr, ShortDescr );

            return ( corpse );
        }

        public void Write( string message ) {
            this.Did( "saw_something", new PythonDictionary { { "message", message } } );
        }

        #region Events
        public override void ReceiveEvent( Event e ) {
            foreach( var ex in e.Parameters.Where( p => p.Value == this )
                                           .Select( e.ChangeToThis ) ) {
                HandleLocalEvent( ex );
                e.ReturnValue = ex.ReturnValue;
            }

            HandleLocalEvent( e );
        }

        private void HandleLocalEvent( Event e ) {
            Contract.Requires( e != null );
            Contract.Requires( Race != null );
            Contract.Requires( Eq != null );

            base.ReceiveEvent( e );
            Race.ReceiveEvent( e );
            Eq.ReceiveEvent( e );

            if( Spec != null )
                Spec.ReceiveEvent( e );

            if( ViewEvent != null )
                ViewEvent( e );
        }


        public virtual void SendEvent( Event e ) {
            Contract.Requires( Room != null );
            Room.ReceiveEvent( e );
        }

        public virtual bool CanDo( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            var canEvent = DoEvent( "ch_can_" + action, EventReturnMethod.And, args );
            return canEvent.ReturnValue;
        }


        public virtual void Does( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }


        public virtual void Did( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }


        private Event DoEvent( string name, EventReturnMethod returnMethod, PythonDictionary args = null ) {
            args = args ?? new PythonDictionary();
            args.Add( "ch", this );
            var doEvent = Entities.Event.Create( name,
                returnMethod,
                args );
            SendEvent( doEvent );

            return doEvent;
        }
        #endregion


        public void SetRoom( Room room ) {
            if( _room != null ) {
                lock( _room ) {
                    UpdateQueue.Detach();
                    _room.RemoveCharacter( this );
                }
            }

            _room = room;

            if( _room != null ) {
                lock( _room ) {
                    _room.AddCharacter( this );
                    UpdateQueue.Attach( _room.UpdateQueue );
                }
            }
        }


        public bool MoveToRoom( Room destination ) {
            Contract.Requires( Room != null );
            Contract.Requires( destination != null );

            if( !Movement.CanLeaveRoom( _room ) )
                return false;
            if( !Movement.CanEnterRoom( destination ) )
                return false;

            var source = _room;
            SetRoom( destination );

            Movement.LeftRoom( source );
            Movement.EnteredRoom( destination );

            return true;
        }


        public virtual CharacterSet GetFoes() {
            return Room.SelectCharacters( vch => !vch.IsSafeFrom( this ) );
        }

        public virtual bool IsSafeFrom( Character vch ) {
            return this == vch;
        }

        public virtual void Kill( Character vch ) {
            Combat.UseAbility( new KillAbility( this, vch ) );
        }

        public virtual void EnqueueAction( string queueName, CharacterAction action ) {
            Contract.Requires( !string.IsNullOrWhiteSpace( queueName ) );
            Contract.Requires( action != null );

            ActionQueueSet.EnqueueAction( queueName, action );
        }

        public virtual void SetTimedEvent( long relativeTime, Action eventAction ) {
            Contract.Requires( relativeTime >= 0 );
            Contract.Requires( eventAction != null );

            UpdateQueue.AddRelative( relativeTime, eventAction );
        }

        public virtual void MakeAttack( IAttack attack ) {
            Contract.Requires( attack != null );
            Contract.Requires( Target != null );

            this.Does( "attacks_ch1", new PythonDictionary { { "ch1", Target }, { "attack", attack } } );
            Combat.MakeAttack( attack );
        }

        public bool Equip( Item item ) {
            Contract.Requires( Room != null );
            Contract.Requires( item.WearLoc != WearLocation.Inventory );
            Contract.Requires( Inventory.Contains( item ) );

            if( !this.CanDo( "equip_item", new PythonDictionary{ { "item", item } } ) )
                return false;

            if( Eq.Have( item.WearLoc ) ) {
                if( !Unequip( Eq.Get( item.WearLoc ) ) )
                    return false;
            }

            Inventory.Remove( item );
            Eq.Equip( item );

            this.Did( "equipped_item", new PythonDictionary { { "item", item } } );

            return true;
        }


        public bool Unequip( Item item ) {
            Contract.Requires( Room != null );
            Contract.Requires( Eq.Have( item ) );

            if( !this.CanDo( "remove_item", new PythonDictionary { { "item", item } } ) )
                return false;

            Eq.Remove( item );
            Inventory.Add( item );

            this.Did( "removed_item", new PythonDictionary { { "item", item } } );
            return true;
        }


        public static Character CreateMob( string vnum ) {
            Contract.Requires( World.Instance.Mobs[vnum] != null );

            var ch = new Character( World.Instance.Mobs[vnum] );
            ch.Initialize();
            ch.Aspects.ai = AspectFactory.AI();
            ch.Restore();
            return ch;
        }

        public static Character Create() {
            var ch = new Character();
            ch.Initialize();
            return ch;
        }

        public static Character Create( Character template, IEventHandler specialization ) {
            return new Character( template ) { Spec = specialization };
        }
    }
}
