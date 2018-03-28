using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Entities;
using CommandSet = System.Collections.Generic.Dictionary<string, Kaerber.MUD.Controllers.Commands.ICommand>;

namespace Kaerber.MUD.Platform.Managers {
    public class CommandManager : IManager<ICommand> {
        private const string DefaultCommandName = "default";

        public IList<string> List( string path ) {
            return _commands.Keys.ToList();
        }

        public ICommand Load( string path, string name ) {
            return _commands[_commands.Keys.Match( name, DefaultCommandName )];
        }

        public void Save( string path, ICommand entity ) {
            _commands[entity.Name] = entity;
        }


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
            { "drop", new Drop() },
            { "wear", new Wear() },
            { "remove", new Remove() },
            { "who", new Who() },
            { "sockets", new Sockets() },
            { "quit", new Quit() },
            { "kill", new Kill() },
            { "default", new UnknownCommand() },
            { "recall", new Recall() },

            { "save", new Save() },
        };
    }
}
