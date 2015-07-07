using System;
using System.Collections.Generic;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Look : Command {
        public override string Name => "look";

        public Look() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>( new List<ArgType>(),
                                                          ( ch, args ) => ch.View.RenderRoom( ch.Model.Room ) ),
                new Tuple<List<ArgType>, CommandHandler>( new List<ArgType> { ArgType.ChRoom },
                                                          ( ch, args ) => ch.View.RenderCharacter( ( Entities.Character )args[0] ) )

            };

        }
    }
}
