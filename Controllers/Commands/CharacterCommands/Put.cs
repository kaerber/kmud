using System;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Put : ICommand {
        public string Name => "put";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            if( input.Arguments.Count() != 2 ) {
                // TODO: train-wreck
                pc.View.Write( "Usage:\t put <object> <container>\n" );
                return;
            }

            var ch = pc.Model;
            var container =  ( Item )Command.MatchArgToType( ch,
                                                             input.Arguments[1],
                                                             ArgType.ObjInv|ArgType.ObjRoom );
            if( container == null ) {
                pc.View.Write( "You do not see that here.\n" );
                return;
            }

            if( container.Container == null ) {
                pc.View.WriteFormat( "You can't put anything into {0}.\n", container.ShortDescr );
                return;
            }

            var item = ( Item )Command.MatchArgToType( ch, input.Arguments[0], ArgType.ObjInv );
            if( item == null ) {
                pc.View.Write( "You do not have it!\n" );
                return;
            }

            if( !ch.Room.Event( "ch_can_put_item_into_container", EventReturnMethod.And,
                                new EventArg( "ch", ch ), 
                                new EventArg( "item", item ), 
                                new EventArg( "container", container ) ) )
                return;

            container.Container.Items.Add( item );
            ch.Inventory.Remove( item );

            ch.Room.Event( "ch_put_item_into_container", EventReturnMethod.None,
                           new EventArg( "ch", ch ), 
                           new EventArg( "item", item ), 
                           new EventArg( "container", container ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
