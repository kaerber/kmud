using System.Collections.Generic;

namespace Kaerber.MUD.Entities {
    public class Exit { // todo rework into portal
        public Exit() {}

        public Exit( string name, string toRoom ) {
            Name = name;
            this.toRoom = toRoom;
        }

        public Exit( string name, Room to ) {
            Name = name;
            To = to;
        }

        public string toRoom { get; set; }

        public Room To {
            get { return _to ?? ( _to = World.Instance.Rooms[toRoom] ); }
            set {
                _to = value;
                toRoom = value.Id;
            }
        }

        public string Name { get; set; }

        public bool GoThrough( Character ch ) {
            ch.SetRoom( _to );
            if( !ch.Can( "enter_room", new EventArg( "room", _to ) ) )
                return false;
            ch.Has( "entered_room", new EventArg( "room", _to ) );
            return true;
        }

        public static Exit Deserialize( dynamic data ) {
            return new Exit {
                Name = data.Name,
                toRoom = data.To
            };
        }

        public static IDictionary<string, object> Serialize( Exit exit ) {
            return new Dictionary<string, object> {
                ["Name"] = exit.Name,
                ["To"] = exit.toRoom
            };
        }


        private Room _to;
    }
}
