using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Go : Command {
        private readonly string _direction;

        public override string Name => "go";

        public Go() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>(
                    new List<ArgType> { ArgType.ExitRoom },
                    ( ch, args ) => GoThroughExit( ch, ( Exit )args[0] ) )
            };

            Messages[0] = "Go where?\n";
            Messages[1] = "You can't go that way.\n";
        }

        public Go( string direction ) : this() {
            _direction = direction;
        }

        public override void Execute( ICharacterController pc, PlayerInput input ) {
            if( !string.IsNullOrWhiteSpace( _direction ) ) {
                input.Arguments = new List<string> { _direction };
                base.Execute( pc, input );
                return;
            }

            base.Execute( pc, input );
        }

        private static void GoThroughExit( ICharacterController pc, Exit exit ) {
            if( pc.Model.IsInCombat ) {
                pc.View.Write( "You can't leave your fight.\n" );
                return;
            }

            var from = pc.Model.Room;
            pc.Model.MoveToRoom( exit.To );

            pc.Model.Movement.WentFromRoom( from, exit.To );

            pc.View.RenderRoom( pc.Model.Room );
        }

    }
}
