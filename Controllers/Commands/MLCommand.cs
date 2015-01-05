using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands
{
    public class MLCommand : ICommand {
        public MLCommand() {
            _function = new MLFunction();
        }

        public MLCommand( string name, string commandText ) : this() {
            _function.Code = commandText;
            Name = name;
        }

        public string Name { get; set; }

        public string Code {
            get { return _function.Code; }
            set { _function.Code = value; }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            _function.Execute( new Dictionary<string, dynamic> {
                { "pc", pc },
                { "self", pc.Model },
                { "input", input },
                { "rawCommand", input.RawLine },
                { "command", input.Command },
                { "rawArgs", input.RawArguments },
                { "args", input.Arguments } 
            } );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }

        private readonly MLFunction _function;
    }
}
