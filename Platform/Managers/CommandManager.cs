using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;

using Microsoft.Practices.ObjectBuilder2;

using CommandSet = System.Collections.Generic.Dictionary<string, Kaerber.MUD.Controllers.Commands.ICommand>;
using Data = System.Collections.Generic.Dictionary<string, dynamic>;

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
            File.WriteAllText( World.CommandsPath, SaveCommands() );
        }

        private void LoadAll() {
            if( _loaded ) return;

            LoadCommands( File.ReadAllText( _root ) );
            _loaded = true;
        }

        private void LoadCommands( string data ) {
            World.Serializer.Deserialize<List<Data>>( data )
                 .Select( Deserialize )
                 .ForEach( command => _commands.Add( command.Name, command ) );
        }

        private string SaveCommands() {
            return World.Serializer.Serialize(
                _commands.Values.OfType<MLCommand>().Select( Serialize ) );
        }


        private bool _loaded;
        private readonly string _root;
        private readonly CommandSet _commands = new CommandSet {
            { "go", new Go() },
            { "north", new Go( "north" ) },
            { "east", new Go( "east" ) },
            { "south", new Go( "south" ) },
            { "west", new Go( "west" ) },
            { "up", new Go( "up" ) },
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

            { "edit", new Edit() }
        };


        private static object Serialize( ICommand command ) {
            return new Data {
                { "name", command.Name },
                { "code", command.Code }
            };
        }

        private static ICommand Deserialize( Data data ) {
            return new MLCommand( data["name"], data["code"] );
        }
    }
}
