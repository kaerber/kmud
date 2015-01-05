using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Kill : Command {
        public override string Name { get { return( "kill" ); } }

        public Kill() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>(
                    new List<ArgType> { ArgType.ChRoom },
                    ( pc, args ) => KillChar( ( Character )args[0] ) )
            };

            Messages[0] = "Kill whom?\n";
            Messages[1] = "You do not see him here.\n";
        }

        protected void KillChar( Character vch ) {
            Self.Kill( vch );
        }
    }
}
