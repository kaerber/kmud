using System;
using System.Collections.Generic;
using System.IO;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;


namespace Kaerber.MUD.Entities {
    public class Area : Entity {
        public List<Room> Rooms;    // TODO: hashtable
        public List<Item> Items;
        public List<Character> Mobs;

        public int ItemId;
        public int RoomId;
        public int MobId;

        public Area() {
            Rooms = new List<Room>();
            Items = new List<Item>();
            Mobs = new List<Character>();
        }

        public Area( string vnum, string names, string shortDescr ) : base( vnum, names, shortDescr ) {
            Rooms = new List<Room>();
            Items = new List<Item>();
            Mobs = new List<Character>();
        }

        public override Entity Initialize() {
            base.Initialize();
            UpdateQueue = new TimedEventQueue( World.Instance.UpdateQueue );
            ClearDirty();
            Rooms.ForEach( room => room.Initialize() );

            return this;
        }

        public override ISerialized Deserialize( IDictionary<string, object> data ) {
            Items = World.ConvertToType<List<Item>>( data["Items"] );
            if( World.Instance != null )
                Items.ForEach( item => {
                    item.Dirty += OnDirty;
                    World.Instance.Items.Add( item.Id, item );
                } );
            ItemId = World.ConvertToTypeExs<int>( data, "itemId" );

            Mobs = World.ConvertToType<List<Character>>( data["Mobs"] );
            if( World.Instance != null )
                Mobs.ForEach( mob => {
                    mob.Dirty += OnDirty;
                    World.Instance.Mobs.Add( mob.Id, mob );
                } );
            MobId = World.ConvertToTypeExs<int>( data, "mobId" );

            Rooms = World.ConvertToType<List<Room>>( data["Rooms"] );
            if( World.Instance != null )
                Rooms.ForEach( room => {
                    room.Dirty += OnDirty;
                    room.Area = this;
                    World.Instance.Rooms.Add( room.Id, room );
                } );
            RoomId = World.ConvertToTypeExs<int>( data, "roomId" );

            return base.Deserialize( data );
        }

        public override IDictionary<string, object> Serialize() {
            return ( base.Serialize()
                .AddEx( "Rooms", Rooms )
                .AddEx( "roomId", RoomId )
                .AddEx( "Items", Items )
                .AddEx( "itemId", ItemId )
                .AddEx( "Mobs", Mobs )
                .AddEx( "mobId", MobId ) );
        }

        public virtual void Save() {
            if( dirty ) {
                lock( this )
                    File.WriteAllText( World.AssetsRootPath + Id + ".data", World.Serializer.Serialize( this ) );
                Console.WriteLine( World.Instance.Time/1000 + ": log area_save: Area " + Id + ": saved" );
            }
            else
                Console.WriteLine( World.Instance.Time/1000 + ": log area_save: Area " + Id + ": save not needed" );

            ClearDirty();
        }

        public void SetDirty() {
            dirty = true;
        }

        public Area ClearDirty() {
            var nextTime = World.Instance.Time + Clock.TimeHour*24*7 + World.RndGen.Next( Clock.TimeHour*24*7 );
            UpdateQueue.Add( nextTime, Save );
            dirty = false;
            return this;
        }


        public Room AddRoom( Room room ) {
            if( string.IsNullOrWhiteSpace( room.Id ) )
                room.Id = string.Format( "{0}_{1:00}", Id, RoomId++ );

            Rooms.Add( room );
            room.Area = this;
            room.Dirty += OnDirty;
            World.Instance.Rooms.Add( room.Id, room );

            return room;
        }


        public Character AddMob( Character mob ) {
            if( string.IsNullOrWhiteSpace( mob.Id ) )
                mob.Id = string.Format( "{0}_{1:00}", Id, MobId++ );
            if( !mob.Id.StartsWith( Id.ToLower() + "_" ) )
                mob.Id = Id.ToLower() + "_" + mob.Id;

            Mobs.Add( mob );
            mob.Dirty += OnDirty;
            World.Instance.Mobs.Add( mob.Id, mob );

            return mob;
        }


        public Item AddItem( Item item ) {
            if( string.IsNullOrWhiteSpace( item.Id ) )
                item.Id = $"{Id}_{ItemId++:00}";
            if( !item.Id.StartsWith( Id.ToLower() + "_" ) )
                item.Id = Id.ToLower() + "_" + item.Id;

            Items.Add( item );
            item.Dirty += OnDirty;
            World.Instance.Items.Add( item.Id, item );

            return item;
        }


        public virtual Room GetRoom( string vnum ) {
            return Rooms.Find( room => room.Id == vnum );
        }


        public static Area Load( string vnum ) {
            return World.Serializer.Deserialize<Area>( File.ReadAllText( World.AssetsRootPath + vnum + ".data" ) );
        }
    }
}
