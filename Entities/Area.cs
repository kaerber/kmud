using System.Collections.Generic;
using System.Diagnostics;

using Kaerber.MUD.Entities.Aspects;


namespace Kaerber.MUD.Entities {
    [DebuggerDisplay( "Area {Id}" )]
    public class Area : Entity {
        public Area() {
            Rooms = new List<Room>();
            Items = new List<Item>();
            Mobs = new List<Character>();
        }

        public void Initialize( TimedEventQueue eventQueue ) {
            base.Initialize();
            UpdateQueue = new TimedEventQueue( eventQueue );
            Rooms.ForEach( room => room.Initialize( UpdateQueue ) );
        }

        public List<Room> Rooms;
        public List<Item> Items;
        public List<Character> Mobs;
    }
}
