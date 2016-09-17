using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities.Aspects;

using log4net;

namespace Kaerber.MUD.Entities {
    public partial class Room : Entity {
        public Room() {
            _exits = new ExitSet();
            Characters = new CharacterSet();
            Items = new ItemSet();
        }

        public RoomReset Resets {
            get { return _resets; }
            set {
                _resets = value;
                if( _resets != null )
                    _resets.Host = this;
            }
        }

        public string Description { get; set; }


        public void Initialize( TimedEventQueue updateQueue ) {
            base.Initialize();
            UpdateQueue = new TimedEventQueue( updateQueue );
        }

        public virtual void AddCharacter( Character ch ) {
            Characters.Add( ch );
            ch.EventSink += ReceiveEvent;
        }

        public virtual void RemoveCharacter( Character ch ) {
            Characters.Remove( ch );
            ch.EventSink -= ReceiveEvent;
        }


        public void Update() {
            UpdateQueue.AddRelative( ( 200 + World.RndGen.Next( 200 ) )*Clock.TimeBase, Update );
            UpdateQueue.Run();

            if( Resets != null )
                lock( this )
                    Resets.Update();

            Log.Info( World.Instance.Time/1000 + ": room " + Id + " updated." );
        }

        public void Round() {
            lock( this )
                Event( "round" );
        }

        public void Tick() {
            lock( this )
                Event( "tick" );
        }


        public Character LoadMob( string vnum ) {
            var mob = Character.CreateMob( vnum );
            mob.SetRoom( this );
            return mob;
        }

        public virtual CharacterSet SelectCharacters( Predicate<Character> predicate ) {
            return new CharacterSet( Characters.Where( predicate ) );
        }

        public override void ReceiveEvent( Event e ) {
            base.ReceiveEvent( e );

            foreach( var ch in Characters )
                ch.Event( e );
        }

        public override string ToString() {
            return $"[{Id ?? "No-ID"}] {ShortDescr ?? "No-Short-Descr"}";
        }

        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public virtual CharacterSet Characters { get; set; }
        public ItemSet Items;
        public long UpdateTime;

        private RoomReset _resets;

        private static readonly ILog Log = LogManager.GetLogger( typeof( Room ) );
    }
}
