using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Platform.Managers {
    public class AreaManager : IManager<Area> {
        public AreaManager( string root,
                            IManager<Character> characterManager,
                            IManager<Item> itemManager ) {
            _root = root;
            _characterManager = characterManager;
            _itemManager = itemManager;
        }

        public IList<string> List( string path ) {
            var areapath = Path.Combine( _root, path );
            return Directory.GetDirectories( areapath )
                            .Select( Path.GetFileName )
                            .ToList();
        }

        public Area Load( string path, string name ) {
            var json = File.ReadAllText( FormPath( path, name ) );
            var area = Deserialize( JsonConvert.DeserializeObject( json ) );

            area.Mobs = Load( path, name, _characterManager );
            area.Items = Load( path, name, _itemManager );
            return area;
        }

        public void Save( string path, Area area ) {
            File.WriteAllText( FormPath( path, area.Id ), JsonConvert.SerializeObject( Serialize( area ) ) );
        }


        public List<T> Load<T>( string path, string name, IManager<T> manager ) {
            var areapath = Path.Combine( path, name );
            return manager.List( areapath )
                          .Select( n => manager.Load( areapath, n ) )
                          .ToList();
        }


        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, name, "area.json" );
        }


        public static Area Deserialize( dynamic data ) {
            var area = new Area();
            EntitySerializer.Deserialize( data, area );


            Func<dynamic, Room> deserializeRoom = roomData => RoomManager.Deserialize( roomData );
            area.Rooms = new List<Room>( Enumerable.Select<dynamic, Room>( data.Rooms, deserializeRoom ) );

            return area;
        }

        public static IDictionary<string, object> Serialize( Area area ) {
            var data = EntitySerializer.Serialize( area );
            data.Add( "Rooms", area.Rooms.Select( RoomManager.Serialize ) );
            return data;
        }


        private readonly string _root;
        private readonly IManager<Character> _characterManager;
        private readonly IManager<Item> _itemManager;
    }
}
