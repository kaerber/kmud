using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands
{
    public class Examine : Command
    {
        public Examine() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>(
                    new List<ArgType> { ArgType.ObjRoom|ArgType.ObjInv },
                    ( ch, args ) => ExamineObject( ch.View, ( Item )args[0] ) )
            };

            Messages[1] = "You do not see that here.\n";

        }

        public override string Name { get { return ( "examine" ); } }

        private void ExamineObject( ICharacterView view, Item obj ) {
            view.RenderItem( obj );
        }
    }
}
