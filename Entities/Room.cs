using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;

using log4net;

namespace Kaerber.MUD.Entities {
    public partial class Room : Entity {
        private string _description;
        private Area _area;
        private RoomReset _resets;

        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public virtual CharacterSet Characters { get; set; }
        public ItemSet Items;
        public long UpdateTime;


        [MudEdit( "Resets" )]
        public RoomReset Resets {
            get { return ( _resets ); }
            set {
                _resets = value;
                if( _resets != null )
                    _resets.Host = this;
            }
        }

        public Area Area {
            get {
                if( _area == null )
                    _area = World.Instance.Areas.Find( area => area.Rooms.Contains( this ) );
                return ( _area );
            }
            set { _area = value; }
        }

        [MudEdit( "Description" )]
        public string Description {
            get { return ( _description ); }
            set {
                _description = value;
                OnDirty();
            }
        }


        public Room() {
            _exits = new ExitSet();
            Characters = new CharacterSet();
            Items = new ItemSet();
        }

        public override Entity Initialize() {
            base.Initialize();
            UpdateQueue = new TimedEventQueue( Area.UpdateQueue );
            Tick();
            Round();
            Update();
            return ( this );
        }

        public override ISerialized Deserialize( IDictionary<string, object> data ) {
            base.Deserialize( data );
            Description = World.ConvertToTypeEx<string>( data, "Description" );
            _exits = new ExitSet( World.ConvertToType<List<Exit>>( data["Exits"] ) );
            Resets = World.ConvertToTypeEx<RoomReset>( data, "Resets" );
            return( this );
        }

        public override IDictionary<string, object> Serialize() {
            return ( base.Serialize()
                .AddIf( "Description", Description,
                        !string.IsNullOrWhiteSpace( Description ) )
                .AddEx( "Exits", _exits )
                .AddIf( "Resets", Resets, Resets != null )
            );
        }

        public virtual void AddCharacter( Character ch ) {
            Characters.Add( ch );
        }

        public virtual void RemoveCharacter( Character ch ) {
            Characters.Remove( ch );
        }


        public void Update() {
            UpdateQueue.AddRelative( ( 200 + World.RndGen.Next( 200 ) )*World.TimeBase, Update );
            UpdateQueue.Run();

            if( Resets != null )
                lock( this )
                    Resets.Update();

            _log.Info( World.Instance.Time/1000 + ": room " + Id + " updated." );
        }

        public void Round() {
            lock( this )
                Event( "round", EventReturnMethod.None );
        }

        public void Tick() {
            lock( this )
                Event( "tick", EventReturnMethod.None );
        }


        public Character LoadMob( string vnum ) {
            Contract.Requires( World.Instance.Mobs[vnum] != null );

            var mob = Character.CreateMob( vnum );
            mob.SetRoom( this );
            return ( mob );
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
            return string.Format( "[{0}] {1}", Id ?? "No-ID", ShortDescr ?? "No-Short-Descr" );
        }

        private static ILog _log = LogManager.GetLogger( typeof( Room ) );
    }
}
