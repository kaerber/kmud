using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Drop : Command {
        public override string Name { get { return( "drop" ); } }

        public Drop() {
            _cmdForms = new List<Tuple<List<ArgType>, CommandHandler>> {
                new Tuple<List<ArgType>, CommandHandler>(
                    new List<ArgType> { ArgType.ObjInv },
                    ( ch, args ) => DropItem( ch.Model, ( Item )args[0] ) )
            };

            Messages[1] = "You do not have that.\n";
        }

        private static void DropItem( Character ch, Item item ) {
            if( !ch.Room.Event( "ch_can_drop_item", EventReturnMethod.And, 
                                new EventArg( "ch", ch ), new EventArg( "item", item ) ) )
                return;

            ch.Inventory.Remove( item );
            ch.Room.Items.Add( item );

            ch.Room.Event( "ch_dropped_item", EventReturnMethod.None,
                           new EventArg( "ch", ch ), new EventArg( "item", item ) );
        }
    }
}
