using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Wear : Command {
        public override string Name { get { return( "wear" ); } }

        public Wear() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>>() {
                new Tuple<List<ArgType>, CommandHandler>(
                    new List<ArgType> { ArgType.ObjInv },
                    ( ch, args ) => ch.Model.Equip( ( Item )args[0] ) )
            };

            Messages[1] = "You do not have that.\n";
        }
    }
}
