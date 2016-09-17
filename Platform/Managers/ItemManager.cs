using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Platform.Managers {
    public class ItemManager : IManager<Item> {
        public ItemManager( string root ) {
            _root = root;
        }

        public IList<string> List( string path ) {
            var itempath = Path.Combine( _root, path, "items" );
            return Directory.GetFiles( itempath )
                            .Select( Path.GetFileNameWithoutExtension )
                            .ToList();
        }

        public Item Load( string path, string name ) {
            var json = File.ReadAllText( FormPath( path, name ) );
            return Deserialize( JsonConvert.DeserializeObject( json ) );
        }

        public void Save( string path, Item item ) {
            File.WriteAllText( FormPath( path, item.Id ),
                               JsonConvert.SerializeObject( Serialize( item ) ) );
        }

        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, "items", name + ".json" );
        }


        public static Item Deserialize( dynamic data ) {
            var item = new Item {
                WearLoc = ( WearLocation )Enum.Parse( typeof( WearLocation ), ( string )data.WearLoc ),
                Cost = data.Cost ?? 0,
            };
            if( data.Flags != null )
                item.Flags = ( ItemFlags )Enum.Parse( typeof( ItemFlags ), ( string )data.Flags );
            if( data.Stack != null )
                item.Stack = Stack.Deserialize( data.Stack );
            if( data.Stats != null )
                item.Stats = AspectFactory.Stats().Deserialize( data.Stats );
            if( data.Weapon != null )
                item.Weapon = AspectFactory.Weapon().Deserialize( data.Weapon );

            EntitySerializer.Deserialize( data, item );
            return item;
        }

        public static IDictionary<string, object> Serialize( Item item ) {
            var data = EntitySerializer.Serialize( item )
                .AddEx( "WearLoc", item.WearLoc )
                .AddEx( "Flags", item.Flags )
                .AddIf( "Stack", item.Stack, item.Stack != null )
                .AddEx( "Cost", item.Cost );

            if( item.Stats != null )
                data.Add( "Stats", item.Stats.Serialize() );
            if( item.Weapon != null )
                data.Add( "Weapon", item.Weapon.Serialize() );
            return data;
        }


        public static Equipment DeserializeEquipment( dynamic data ) {
            Func<dynamic, WearLocation> keySelector = 
                itemData => Enum.Parse( typeof( WearLocation ), itemData.Name );
            Func<dynamic, Item> valueSelector = 
                itemData => Deserialize( itemData.First );

            return new Equipment( Enumerable.ToDictionary( data, keySelector, valueSelector ) );
        }

        public static IDictionary<string, object> Serialize( Equipment equipment ) {
            return equipment.Stuff.Keys.Count > 0
                       ? equipment.Stuff.ToDictionary(
                           pair => Enum.GetName( typeof( WearLocation ), pair.Key ),
                           pair => ( object )pair.Value )
                       : null;
        }


        private readonly string _root;
    }
}
