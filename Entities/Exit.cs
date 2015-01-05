using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class Exit : ISerialized {
        private Room _to;

        public Exit() {}

        public Exit( string name, string toRoom ) {
            Name = name;
            this.toRoom = toRoom;
        }

        public Exit( string name, Room to ) {
            Name = name;
            To = to;
        }

        public ISerialized Deserialize( IDictionary<string, object> data ) {
            Name = World.Serializer.ConvertToType<string>( data[ "Name" ] );
            toRoom = World.Serializer.ConvertToType<string>( data[ "To" ] );

            return ( this );
        }

        public IDictionary<string, object> Serialize() {
            return ( new Dictionary<string, object> {
                    { "Name", Name },
                    { "To", toRoom }
                }
            );
        }

        [MudEdit( "Leads to room" )]
        public string toRoom { get; set; }

        public Room To {
            get { return ( _to ?? ( _to = World.Instance.Rooms[toRoom] ) ); }
            set {
                _to = value;
                toRoom = value.Id;
            }
        }

        [MudEdit( "Name of the exit" )]
        public string Name { get; set; }
    }
}
