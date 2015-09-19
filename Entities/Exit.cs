using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class Exit {
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

        public string toRoom { get; set; }

        public Room To {
            get { return ( _to ?? ( _to = World.Instance.Rooms[toRoom] ) ); }
            set {
                _to = value;
                toRoom = value.Id;
            }
        }

        public string Name { get; set; }


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
    }
}
