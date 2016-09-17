using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities {
    public class World {
        public World( IManager<Area> areaManager, Clock clock ) {
            _areaManager = areaManager;
            _clock = clock;

            Areas = new List<Area>();
            Rooms = new Dictionary<string, Room>();
            Mobs = new Dictionary<string, Character>();
            Items = new Dictionary<string, Item>();

            _updateQueue = new TimedEventQueue( null );

            InitClock( _clock );
        }

        public long Time => _clock.Time;

        public virtual void Pulse( long ticks ) {
            _clock.Pulse( ticks );
        }

        public void LoadAreas() {
            Areas = _areaManager.List( "areas" )
                               .Select( name => _areaManager.Load( "areas", name ) )
                               .ToList();
            Areas.ForEach( a => a.Initialize( _updateQueue ) );

            Rooms = Areas.SelectMany( a => a.Rooms ).ToDictionary( r => r.Id, r => r );
            Mobs = Areas.SelectMany( a => a.Mobs ).ToDictionary( m => m.Id, m => m );
            Items = Areas.SelectMany( a => a.Items ).ToDictionary( i => i.Id, i => i );

            Rooms.Values.ForEach( r => r.Update() );
        }

        public void ReceiveEvent( Event e ) {
            
        }

        private void InitClock( Clock clock ) {
            clock.Update += _updateQueue.Run;
            clock.Round += () => Rooms.Values.ForEach( room => room.Round() );
            clock.Hour += () => Rooms.Values.ForEach( room => room.Tick() );
        }

        public List<Area> Areas;
        public Dictionary<string, Room> Rooms;
        public Dictionary<string, Character> Mobs;
        public Dictionary<string, Item> Items;

        private readonly Clock _clock;
        private readonly TimedEventQueue _updateQueue;
        private readonly IManager<Area> _areaManager;

        public static World Instance;
        public static Random RndGen = new Random();


    }
}
