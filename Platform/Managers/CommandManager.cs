using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;
using Newtonsoft.Json;
using CommandSet = System.Collections.Generic.Dictionary<string, Kaerber.MUD.Controllers.Commands.ICommand>;

namespace Kaerber.MUD.Platform.Managers {
    public class CommandManager : IManager<ICommand> {
        private const string DefaultCommandName = "default";

        public CommandManager( string root ) {
            _root = root;
            _loaded = false;
        }

        public IList<string> List( string path ) {
            LoadAll();
            return _commands.Keys.ToList();
        }

        public ICommand Load( string path, string name ) {
            LoadAll();
            return _commands[_commands.Keys.Match( name, DefaultCommandName )];
        }

        public void Save( string path, ICommand entity ) {
            _commands[entity.Name] = entity;
            File.WriteAllText( _root, SaveCommands() );
        }

        private void LoadAll() {
            if( _loaded ) return;

            LoadCommands( File.ReadAllText( _root ) );
            _loaded = true;
        }

        private void LoadCommands( string data ) {
            Func<dynamic, MLCommand> deserializeCommand = d => Deserialize( d );

            dynamic json = JsonConvert.DeserializeObject( data );
            foreach( var command in Enumerable.Select( json, deserializeCommand ) )
                _commands.Add( command.Name, command );
        }

        private string SaveCommands() {
            return JsonConvert.SerializeObject(
                _commands.Values.OfType<MLCommand>().Select( Serialize ) );
        }


        private bool _loaded;
        private readonly string _root;
        private readonly CommandSet _commands = new CommandSet {
            { "go", new Go() },
            { "n", new Go( "north" ) },
            { "north", new Go( "north" ) },
            { "e", new Go( "east" ) },
            { "east", new Go( "east" ) },
            { "s", new Go( "south" ) },
            { "south", new Go( "south" ) },
            { "w", new Go( "west" ) },
            { "west", new Go( "west" ) },
            { "u", new Go( "up" ) },
            { "up", new Go( "up" ) },
            { "d", new Go( "down" ) },
            { "down", new Go( "down" ) },
            { "look", new Look() },
            { "examine", new Examine() },
            { "say", new Say() },
            { "tell", new Tell() },
            { "reply", new Reply() },
            { "inventory", new Inventory() },
            { "equipment", new ShowEquipment() },
            { "get", new Get() },
            { "put", new Put() },
            { "drop", new Drop() },
            { "wear", new Wear() },
            { "remove", new Remove() },
            { "who", new Who() },
            { "sockets", new Sockets() },
            { "quit", new Quit() },
            { "kill", new Kill() },
            { "default", new UnknownCommand() },

            { "save", new Save() },
        };


        private static Dictionary<string, object> Serialize( ICommand command ) {
            return new Dictionary<string, object> {
                { "name", command.Name },
                { "code", command.Code }
            };
        }

        private static MLCommand Deserialize( dynamic data ) {
            return new MLCommand( data.Path, ( string )data.First );
        }
    }
}
