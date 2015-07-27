using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

using Newtonsoft.Json;

namespace Kaerber.MUD.Platform.Managers {
    public class AreaManager : IManager<Area> {
        public AreaManager( string root, IManager<Character> characterManager ) {
            _root = root;
            _characterManager = characterManager;
        }

        public IList<string> List( string path ) {
            throw new System.NotImplementedException();
        }

        public Area Load( string path, string name ) {
            var json = File.ReadAllText( FormPath( path, name ) );
            var area = Deserialize( JsonConvert.DeserializeObject<Dictionary<string, object>>( json ) );

            area.Mobs = LoadMobs( path, name );
            if( World.Instance != null )
                area.Mobs.ForEach( mob => {
                    mob.Dirty += area.OnDirty;
                    World.Instance.Mobs.Add( mob.Id, mob );
                } );

            return area;
        }

        public void Save( string path, Area area ) {
            File.WriteAllText( FormPath( path, area.Id ), JsonConvert.SerializeObject( Serialize( area ) ) );
        }

        public Area Deserialize( IDictionary<string, object> data ) {
            var area = new Area();
            if( data.ContainsKey( "Aspects" ) )
                area.Aspects.Deserialize( data["Aspects"] );

            area.Id = ( string )data["Vnum"];
            /*area.Names = World.ConvertToTypeEx<string>( data, "Names" );
            area.ShortDescr = World.ConvertToTypeEx<string>( data, "ShortDescr" );

            area.Affects = new AffectSet(
                World.ConvertToTypeEx( data, "Affects", new List<Affect>() ),
                area );

            area.Handlers = World.ConvertToTypeEx( data, "Handlers", new HandlerSet() );

            area.Items = World.ConvertToType<List<Item>>( data["Items"].ToString() );
            if( World.Instance != null )
                area.Items.ForEach( item => {
                    item.Dirty += area.OnDirty;
                    World.Instance.Items.Add( item.Id, item );
                } );
            area.ItemId = World.ConvertToTypeExs<int>( data, "itemId" );

            area.MobId = World.ConvertToTypeExs<int>( data, "mobId" );

            area.Rooms = World.ConvertToType<List<Room>>( data["Rooms"].ToString() );
            if( World.Instance != null )
                area.Rooms.ForEach( room => {
                    room.Dirty += area.OnDirty;
                    room.Area = area;
                    World.Instance.Rooms.Add( room.Id, room );
                } );
            area.RoomId = World.ConvertToTypeExs<int>( data, "roomId" );*/

            return area;
        }

        public IDictionary<string, object> Serialize( Area area ) {
            var affToSave = area.Affects.Where( aff => !aff.Flags.HasFlag( AffectFlags.NoSave ) );
            var data = new Dictionary<string, object> {
                { "Vnum", area.Id },
                { "Names", area.Names },
                { "ShortDescr", area.ShortDescr },
            }
                .AddIf( "Affects", affToSave, affToSave.Any() )
                .AddIf( "Handlers", area.Handlers, area.Handlers.Count > 0 );

            data.Add( "Aspects", area.Aspects.Serialize() );
            return data.AddEx( "roomId", area.RoomId )
                       .AddEx( "Items", area.Items )
                       .AddEx( "itemId", area.ItemId )
                       .AddEx( "Mobs", area.Mobs )
                       .AddEx( "mobId", area.MobId );
        }

        public List<Character> LoadMobs( string path, string name ) {
            var areapath = Path.Combine( path, name );
            return _characterManager.List( areapath )
                                    .Select( n => _characterManager.Load( areapath, n ) )
                                    .ToList();
        }

        public void SaveMobs( string path, string name, List<Character> mobs ) {
            var areapath = Path.Combine( path, name );
            mobs.ForEach( mob => _characterManager.Save( areapath, mob ) );
        }

        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, name, "area.data" );
        }

        private readonly string _root;
        private readonly IManager<Character> _characterManager;
    }
}
