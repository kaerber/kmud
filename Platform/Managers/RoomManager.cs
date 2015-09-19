using System.Collections.Generic;
using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Platform.Managers {
    public class RoomManager : IManager<Room> {
        public IList<string> List( string path ) {
            throw new System.NotImplementedException();
        }

        public Room Load( string path, string name ) {
            throw new System.NotImplementedException();
        }

        public void Save( string path, Room entity ) {
            throw new System.NotImplementedException();
        }

        public static Room Deserialize( dynamic data ) {
            var room = new Room {
                Description = data.Description
            };

            if( data.Exits != null )
                room._exits = ExitSet.Deserialize( data.Exits );

            if( data.Resets != null )
                room.Resets = RoomReset.Deserialize( data.Resets );
            EntitySerializer.Deserialize( data, room );
            return room;
        }

        public static IDictionary<string, object> Serialize( Room room ) {
            var data = EntitySerializer.Serialize( room );
            data.Add( "Description", room.Description );
            data.Add( "Exits", ExitSet.Serialize( room._exits) );
            if( room.Resets != null )
                data.Add( "Resets", RoomReset.Serialize( room.Resets ) );

            return data;
        }
    }
}
