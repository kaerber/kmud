﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using IronPython.Runtime;

using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities
{
    public class Character : Entity {
        public Character() : this( new CharacterCore() ) {}

        public Character( CharacterCore core ) {
            _core = core;

            NaturalWeapon = AspectFactory.Weapon();
            NaturalWeapon.BaseDamage = 1;

            Setup();
        }

        public Character( Character template, CharacterCore core ) 
            : base( template.Id, template.Names, template.ShortDescr ) {

            _core = core;
            NaturalWeapon = template.NaturalWeapon.Clone();
            Setup();
            if( template.Aspects != null )
                Aspects = template.Aspects.Clone();
        }

        private void Setup() {
            Race = RaceFactory.Default;
            Spec = SpecFactory.Warrior;

            Inventory = new ItemSet();
            Eq = new Equipment();
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

        public bool IsInCombat => Target != null;
        public virtual Character Target => Combat.Target;

        private Equipment _eq;

        public Room RespawnAt;
        public string RespawnAtId { get; set; }

        public Room Room {
            get { return _room; }
            private set {
                _room = value;
                RoomId = value?.Id;
            }
        }

        public string RoomId { get; set; }

        public ItemSet Inventory;

        public Dictionary<string, string> Data;

        public Action<Event> ViewEvent;


        public Equipment Eq {
            get { return _eq; }
            set {
                _eq = value;
                _eq.Host = this;
            }
        }

        public dynamic NaturalWeapon { get; set; }

        [Obsolete]
        public dynamic Health => Aspects.health;

        [Obsolete]
        public dynamic Movement => Aspects.movement;

        private dynamic Combat => Aspects.combat;

        public dynamic Stats => Aspects.stats;

        public ActionQueueSet ActionQueueSet { get; set; }

        public override string ToString() { return ShortDescr; }

        public void Restore() {
            Health.Restore();
        }


        public void Die() {
            Contract.Requires( Room != null );

            if( !this.Can( "die" ) )
                return;
            
            var corpse = CreateCorpse();
            corpse.Container.Items.AddRange( Inventory );
            Inventory.Clear();
            corpse.Container.Items.AddRange( Eq.Items );
            Eq.Clear();

            this.Has( "died" );
            SetRoom( RespawnAt );
            if( RespawnAt != null )
                Restore();
        }


        protected Item CreateCorpse() {
            var corpse = Room.Items.Load( "ch_corpse" );
            corpse.Names = string.Format( corpse.Names, Names );
            corpse.ShortDescr = string.Format( corpse.ShortDescr, ShortDescr );

            return corpse;
        }

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

            base.ReceiveEvent( e );

            Race?.ReceiveEvent( e );
            Spec?.ReceiveEvent(e);

            Eq?.ReceiveEvent( e );
            NaturalWeapon?.ReceiveEvent( e );

            ViewEvent?.Invoke( e );
        }


        public virtual void SendEvent( Event e ) {
            Contract.Requires( Room != null );
            Room.ReceiveEvent( e );
        }

        public virtual bool Can( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            var canEvent = DoEvent( "ch_can_" + action, EventReturnMethod.And, args );
            return canEvent.ReturnValue;
        }


        public virtual void Is( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }


        public virtual void Has( string action, PythonDictionary args = null ) {
            Contract.Requires( Room != null );
            DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }


        private Event DoEvent( string name, EventReturnMethod returnMethod, PythonDictionary args = null ) {
            args = args ?? new PythonDictionary();
            args.Add( "ch", this );
            var doEvent = Entities.Event.Create( name, returnMethod, args );
            SendEvent( doEvent );

            return doEvent;
        }


        public void SetRoom( Room room ) {
            if( Room != null ) {
                lock( Room ) {
                    UpdateQueue.Detach();
                    Room.RemoveCharacter( this );
                }
            }

            Room = room;

            if( Room != null ) {
                lock( Room ) {
                    Room.AddCharacter( this );
                    UpdateQueue.Attach( Room.UpdateQueue );
                }
            }
        }


        public bool MoveToRoom( Room destination ) {
            Contract.Requires( Room != null );
            Contract.Requires( destination != null );

            if( !Movement.CanLeaveRoom( Room ) )
                return false;
            if( !Movement.CanEnterRoom( destination ) )
                return false;

            var source = Room;
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

            this.Is( "attacks_ch1", new PythonDictionary { { "ch1", Target }, { "attack", attack } } );
            Combat.MakeAttack( attack );
        }

        public bool Equip( Item item ) {
            Contract.Requires( Room != null );
            Contract.Requires( item.WearLoc != WearLocation.Inventory );
            Contract.Requires( Inventory.Contains( item ) );

            if( !this.Can( "equip_item", new PythonDictionary{ { "item", item } } ) )
                return false;

            if( Eq.Have( item.WearLoc ) ) {
                if( !Unequip( Eq.Get( item.WearLoc ) ) )
                    return false;
            }

            Inventory.Remove( item );
            Eq.Equip( item );

            this.Has( "equipped_item", new PythonDictionary { { "item", item } } );

            return true;
        }


        public bool Unequip( Item item ) {
            Contract.Requires( Room != null );
            Contract.Requires( Eq.Have( item ) );

            if( !this.Can( "remove_item", new PythonDictionary { { "item", item } } ) )
                return false;

            Eq.Remove( item );
            Inventory.Add( item );

            this.Has( "removed_item", new PythonDictionary { { "item", item } } );
            return true;
        }

        public static Character CreateMob( string vnum ) {
            var ch = new Character( World.Instance.Mobs[vnum], new CharacterCore() );
            ch.Initialize();
            ch.Aspects.ai = AspectFactory.AI();
            ch.Restore();
            return ch;
        }


        private readonly CharacterCore _core;
        private Room _room;
    }
}
