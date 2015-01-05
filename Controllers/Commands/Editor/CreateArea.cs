using System;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class CreateArea : ICommand {
        public string Name {
            get { return "CreateArea"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            if( input.Arguments.Count != 3 ) {
                pc.View.Write( "Usage: createarea <vnum> \"<names>\" \"<short description>\"\n" );
                return;
            }

            var area = new Area( input.Arguments[ 0 ], input.Arguments[ 1 ], input.Arguments[ 2 ] );
            World.Instance.Areas.Add( area );
            var room = area.AddRoom( new Room { Id = area.Id + "_001", Names = area.Names, ShortDescr = area.ShortDescr } );
            pc.Model.SetRoom( room );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
