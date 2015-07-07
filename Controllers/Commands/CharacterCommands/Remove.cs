using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Remove : Command {
        public override string Name => "remove";

        public Remove() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>( new List<ArgType> { ArgType.ObjEq },
                                                         ( ch, args ) => ch.Model.Eq.Remove( ( Item )args[0] ) )
            };

            Messages[1] = "You do not have that.\n";
        }
    }
}
