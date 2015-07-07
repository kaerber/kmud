using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class Dig : ICommand {
        // Usage: dig <direction> <room_vnum>
        public string Name => "dig";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
//            var editor = ( ( CharacterController )pc ).Editor;
//            var room = ( Room )editor.Value;
//            var roomNew = ( Room )editor.Create( input.Arguments.Count() >=2 
//                ? input.Arguments.ElementAt( 1 ) 
//                : string.Empty );
//            roomNew.Description = room.Description;
//            roomNew.Names = room.Names;
//            roomNew.ShortDescr = room.ShortDescr;
//
//            var exitNames = Connect( room, roomNew, input.Arguments[0].ToLower() );
//
//            editor.ChangeTo( roomNew.Id );
//            pc.View.WriteFormat( "You dig {0}. Room {1} created.\n", exitNames.Item1, roomNew.Id );
        }

        public static Tuple<string, string> Connect( Room roomFrom, Room roomTo, string exitName ) {
            var start = _opposite.Keys.FirstOrDefault( dir => dir.StartsWith( exitName ) ) ?? exitName;
            var finish = roomFrom.Id;
            if( _opposite.ContainsKey( start ) )
                finish = _opposite[start];
            roomFrom.AddExit( start, roomTo );
            roomTo.AddExit( finish, roomFrom );

            return new Tuple<string, string>( start, finish );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }

        private static readonly Dictionary<string, string> _opposite = new Dictionary<string, string> {
            { "north", "south" },
            { "south", "north" },
            { "east", "west" },
            { "west", "east" },
            { "up", "down" },
            { "down", "up" }
        };
     
    }
}
