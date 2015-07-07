using System;
using System.Linq;

using IronPython.Runtime;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Get : ICommand {
        public string Name => "get";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            switch( input.Arguments.Count() ) {
                case 1: // item in room
                    var roomItem = pc.Model.Room.Items.Find( input.Arguments.ElementAt( 0 ) );
                    if( roomItem == null ) {
                        pc.View.Write( "You do not see that here.\n" );
                        return;
                    }

                    CharacterGetsItem( pc.Model, roomItem );
                    break;

                case 2: // item in container
                    var container =  pc.Model.Inventory.Union( pc.Model.Room.Items )
                        .FirstOrDefault( obj => obj.MatchNames( input.Arguments.ElementAt( 1 ) ) );
                    if( container == null ) {
                        pc.View.Write( "You do not see that here.\n" );
                        return;
                    }
                    var containerItem = container.Container.FindItem( input.Arguments.ElementAt( 0 ) );
                    if( containerItem == null ) {
                        pc.View.Write( $"{container.ShortDescr} does not contain it!\n" );
                        return;
                    }
                    CharacterGetsItemFromContainer( pc.Model, container, containerItem );
                    break;

                default:
                    pc.View.Write( "Usage:\t get <item>\n" );
                    pc.View.Write( "      \t get <item> <container>\n" );
                    break;
            }
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }

        private static void CharacterGetsItemFromContainer( Character ch, Item container, Item containerItem ) {
            if( !ch.Can( "get_item_from_container", 
                         new PythonDictionary {
                                { "ch", ch },
                                { "item", containerItem }, 
                                { "container", container }
                         } ) )
                return;

            container.Container.Items.Remove( containerItem );
            ch.Inventory.Add( containerItem );

            ch.Has( "got_item_from_container",
                    new PythonDictionary { 
                        { "ch", ch },
                        { "item", containerItem },
                        { "container", container }
                    } );
        }


        private static void CharacterGetsItem( Character ch, Item item ) {
            if( !ch.Can( "get_item", new PythonDictionary { { "ch", ch }, { "item", item } } ) )
                return;

            ch.Room.Items.Remove( item );
            ch.Inventory.Add( item );

            ch.Has( "got_item", new PythonDictionary { { "ch", ch }, { "item", item } } );
        }
    }
}
