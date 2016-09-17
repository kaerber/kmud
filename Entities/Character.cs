using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Runtime;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;
using static Kaerber.MUD.Entities.Event;

namespace Kaerber.MUD.Entities {
    public class Character : Entity, IEventSource {
        public Character() {
            NaturalWeapon = AspectFactory.Weapon();
            NaturalWeapon.BaseDamage = 1;

            Setup();
        }

        public Character( Character template )
            : base( template.Id, template.Names, template.ShortDescr ) {
            NaturalWeapon = template.NaturalWeapon.Clone();
            Setup();
            if( template.Aspects != null )
                Aspects = template.Aspects.Clone();
        }

        public IEventTarget Class { get; set; }

        public bool IsInCombat => Target != null;
        public virtual Character Target => Combat.Target;
        public string RespawnAtId { get; set; }

        public Room Room
        {
            get { return _room; }
            set
            {
                _room = value;
                RoomId = value?.Id;
            }
        }

        public string RoomId { get; set; }

        public object Sync => Room != null ? ( object )Room : this;


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

        private dynamic Combat => Aspects.combat;

        public dynamic Stats => Aspects.stats;

        public ActionQueueSet ActionQueueSet { get; set; }

        private void Setup() {
            Class = ClassFactory.Warrior;

            Inventory = new ItemSet();
            Eq = new Equipment();
            Data = new Dictionary<string, string>();

            ActionQueueSet = new ActionQueueSet( this );
            UpdateQueue = new TimedEventQueue( null );

            Aspects.stats = AspectFactory.Stats();
            Aspects.health = AspectFactory.Health();
            Aspects.combat = AspectFactory.Combat();
        }

        public override string ToString() {
            return ShortDescr;
        }

        public void Restore() {
            Health.Restore();
        }


        public void Die() {
            if( !this.Can( "die" ) )
                return;

            Inventory.Clear();
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
            foreach( var ex in e.Parameters.Where( p => p.Value is Character && p.Value == this )
                                .Select( e.ChangeToThis ) ) {
                HandleLocalEvent( ex );
                e.ReturnValue = ex.ReturnValue;
            }

            HandleLocalEvent( e );
        }

        private void HandleLocalEvent( Event e ) {
            base.ReceiveEvent( e );

            Class?.ReceiveEvent( e );

            Eq?.ReceiveEvent( e );
            NaturalWeapon?.ReceiveEvent( e );

            ViewEvent?.Invoke( e );
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

        public void Action( string name, EventArg[] args, Action action ) {
            if( !this.Can( name, args ) )
                return;
            action();
            this.Has( name, args );
        }

        public bool MoveToRoom( Exit exit ) {
            var roomFrom = new EventArg( "room", Room );

            if( !this.Can( "leave_room", roomFrom ) )
                return false;
            this.Has( "left_room", roomFrom );

            if( exit.GoThrough( this ) )
                return true;

            SetRoom( roomFrom.Value );
            this.Has( "entered_room", roomFrom );
            return false;
        }

        public void WentFromRoom( Room from, Room to ) {    // todo: remove event
            var @event = Create( "ch_went_from_room_to_room",
                                 EventReturnMethod.None,
                                 new EventArg( "ch", this ),
                                 new EventArg( "room_from", from ),
                                 new EventArg( "room_to", to ),
                                 new EventArg( "exit", from.Exits[to] ),
                                 new EventArg( "entrance", to.Exits[from] ) );
            from.ReceiveEvent( @event );
            to.ReceiveEvent( @event );
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
            ActionQueueSet.EnqueueAction( queueName, action );
        }

        public virtual void SetTimedEvent( long relativeTime, Action eventAction ) {
            UpdateQueue.AddRelative( relativeTime, eventAction );
        }

        public virtual void MakeAttack( IAttack attack ) {
            this.Is( "attacking_ch1", new PythonDictionary { { "ch1", Target }, { "attack", attack } } );
            Combat.MakeAttack( attack );
        }

        public bool Equip( Item item ) {
            if( !this.Can( "equip_item", new PythonDictionary { { "item", item } } ) )
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
            if( !this.Can( "remove_item", new PythonDictionary { { "item", item } } ) )
                return false;

            Eq.Remove( item );
            Inventory.Add( item );

            this.Has( "removed_item", new PythonDictionary { { "item", item } } );
            return true;
        }


        public static Character CreateMob( string vnum ) {
            var ch = new Character( World.Instance.Mobs[vnum] );
            ch.Initialize();
            ch.Aspects.ai = AspectFactory.AI();
            ch.Restore();
            return ch;
        }


        private Equipment _eq;
        private Room _room;

        public Dictionary<string, string> Data;

        public ItemSet Inventory;

        public Room RespawnAt;

        public Action<Event> ViewEvent;
    }
}