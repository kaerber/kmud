using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Platform.Managers {
    public class CharacterManager : IManager<Character> {
        public CharacterManager( string root, IManager<IAbility> abilityManager ) {
            _root = root;
            _abilityManager = abilityManager;
        }

        public IList<string> List( string path ) {
            var charpath = Path.Combine( _root, path, "characters" );
            return Directory.GetDirectories( charpath )
                            .Select( Path.GetFileName )
                            .ToList();
        }

        public Character Load( string path, string name ) {
            var data = LoadData( path, name );
            return Deserialize( data );
        }

        public void Save( string path, Character character ) {
            File.WriteAllText( FormPath( path, character.ShortDescr ),
                               JsonConvert.SerializeObject( Serialize( character ) ) );
        }

        public static Character Create() {
            var ch = new Character();
            ch.Initialize();
            return ch;
        }

        public static Character Deserialize( dynamic data ) {
            var character = new Character();
            EntitySerializer.Deserialize( data, character );

            if( data.Stats != null )
                character.Stats.Deserialize( data.Stats );
            if( data.NaturalWeapon != null )
                character.NaturalWeapon.Deserialize( data.NaturalWeapon );

            Func<dynamic, Item> deserializeItem = itemData => ItemManager.Deserialize( itemData );

            if( data.Inventory != null )
                character.Inventory = new ItemSet( Enumerable.Select( data.Inventory, deserializeItem ) );
            if( data.Equipment != null )
                character.Eq = ItemManager.DeserializeEquipment( data.Equipment );

            if( data.RespawnAt != null )
                character.RespawnAtId = data.RespawnAt;

            if( data.LoginAt != null )
                character.RoomId = data.LoginAt;

            if( data.Data != null ) {
                Func<dynamic, string> keySelector = item => item.Name;
                Func<dynamic, string> valueSelector = item => item.First;
                character.Data = Enumerable.ToDictionary( data.Data, keySelector, valueSelector );
            }

            return character;
        }

        public static IDictionary<string, object> Serialize( Character character ) {
            var data = EntitySerializer.Serialize( character )
                .AddIf( "Inventory", character.Inventory, character.Inventory.Count > 0 )
                .AddIf( "Equipment", character.Eq, character.Eq.Count > 0 )

                .AddIf( "LoginAt", character.RoomId, character.RoomId != null )
                .AddIf( "RespawnAt", character.RespawnAtId, character.RespawnAtId != null )

                .AddIf( "Data", character.Data, character.Data != null && character.Data.Keys.Count > 0 );

            data.Add( "Stats", character.Stats.Serialize() );
            data.Add( "NaturalWeapon", character.NaturalWeapon.Serialize() );

            return data;
        }

        public static Character Create( Character template, IEventTarget specialization ) {
            return new Character( template ) { Spec = specialization };
        }

        public dynamic LoadData( string path, string name ) {
            var json = File.ReadAllText( FormPath( path, name ) );
            return JsonConvert.DeserializeObject( json );
        }


        private string FormPath( string path, string name ) {
            return Path.Combine( CharacterDirectory( path, name ), "character.json" );
        }

        private string CharacterDirectory( string path, string name ) {
            return Path.Combine( _root, path, "characters", name );
        }


        private readonly string _root;
        private readonly IManager<IAbility> _abilityManager;
    }
}
