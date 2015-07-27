using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Newtonsoft.Json;

namespace Kaerber.MUD.Platform.Managers {
    public class CharacterManager : IManager<Character> {
        public CharacterManager( string root ) {
            _root = root;
        }

        public IList<string> List( string path ) {
            var charpath = Path.Combine( _root, path, "characters" );
            return Directory.GetFiles( charpath )
                            .Select( Path.GetFileNameWithoutExtension )
                            .ToList();
        }

        public Character Load( string path, string name) {
            var json = File.ReadAllText( FormPath( path, name ) );
            return Deserialize( JsonConvert.DeserializeObject<Dictionary<string, object>>( json ) );
        }

        public void Save( string path, Character character ) {
            File.WriteAllText( FormPath( path, character.ShortDescr ),
                               World.Serializer.Serialize( character ) );
        }

        public static IDictionary<string, object> Serialize( Character character ) {
            var affToSave = character.Affects.Where(aff => !aff.Flags.HasFlag(AffectFlags.NoSave));
            var data = new Dictionary<string, object> {
                    { "Vnum", character.Id },
                    { "Names", character.Names },
                    { "ShortDescr", character.ShortDescr },
                }
                .AddIf("Affects", affToSave, affToSave.Any())
                .AddIf("Handlers", character.Handlers, character.Handlers.Count > 0 )
                .AddEx("Flags", character.Flags )
                .AddIf("Inventory", character.Inventory, character.Inventory.Count > 0 )
                .AddIf("Equipment", character.Eq, character.Eq.Count > 0)

                .AddIf("LoginAt", character.Room != null ? character.Room.Id : string.Empty, 
                                  character.Room != null )
                .AddIf("RespawnAt", character.RespawnAt != null ? character.RespawnAt.Id : string.Empty, 
                                    character.RespawnAt != null )

                .AddIf("Data", character.Data, character.Data != null && character.Data.Keys.Count > 0 );

            data.Add("Aspects", character.Aspects.Serialize() );
            data.Add("Stats", character.Stats.Serialize() );
            data.Add("NaturalWeapon", character.NaturalWeapon.Serialize());

            return data;
        }

        public static Character Deserialize( IDictionary<string, object> data ) {
            var core = new CharacterCore();
            var character = new Character( core );

            if( data.ContainsKey( "Aspects" ) )
                character.Aspects.Deserialize( data["Aspects"] );

            character.Id = World.ConvertToType<string>( data["Vnum"] );
            character.Names = World.ConvertToTypeEx<string>( data, "Names" );
            character.ShortDescr = World.ConvertToTypeEx<string>( data, "ShortDescr" );

            character.Affects = new AffectSet(
                World.ConvertToTypeEx( data, "Affects", new List<Affect>() ),
                character );

            character.Handlers = World.ConvertToTypeEx( data, "Handlers", new HandlerSet() );

            if( data.ContainsKey( "Stats" ) )
                character.Stats.Deserialize( data["Stats"] );

            if( data.ContainsKey( "NaturalWeapon" ) )
                character.NaturalWeapon.Deserialize( data["NaturalWeapon"] );

            character.Flags = World.ConvertToTypeExs<MobFlags>( data, "Flags" );

            character.Inventory = new ItemSet(
                World.ConvertToTypeEx( data, "Inventory", new List<Item>() ) );
            character.Eq = World.ConvertToTypeEx( data, "Equipment", new Equipment() );

            var respawnAt = World.ConvertToTypeEx<string>( data, "RespawnAt" );
            //character.RespawnAt = world.GetRoom( respawnAt );

            var loginAt = World.ConvertToTypeEx<string>( data, "LoginAt" );
            //character.SetRoom( world.GetRoom( loginAt ) );
            //if( character.Room == null )
            //    character.SetRoom( character.RespawnAt );

            character.Data = World.ConvertToTypeEx( data, "Data", new Dictionary<string, string>() );

            return character;
        }

        public static Character Create( CharacterCore core ) {
            var ch = new Character( core );
            ch.Initialize();
            return ch;
        }

        public static Character Create( Character template, CharacterCore core, IEventHandler specialization ) {
            return new Character( template, core ) { Spec = specialization };
        }


        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, "characters", name + ".data" );
        }


        private readonly string _root;
    }
}
