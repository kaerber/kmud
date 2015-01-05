using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    public class MobReset {
        [MudEdit( "Vnum" )]
        public string Vnum { get; set; }

        [MudEdit( "Equipment" )]
        public List<MobObjReset> Eq { get; set; }

        public void Update( Room host ) {
            if( host.Characters.Count( ch => ch.Id == Vnum ) >= 1 )
                return;

            if( string.IsNullOrWhiteSpace( Vnum ) ) {
                Console.WriteLine( "Empty vnum in MobReset in room {0}, skipping.", host.Id );
                return;
            }

            if( !World.Instance.Mobs.ContainsKey( Vnum ) ) {
                Console.WriteLine( "Unknown mob vnum {0} in MobReset in room {1}, skipping.", Vnum, host.Id );
                return;
            }

            var mob = host.LoadMob( Vnum );
            if( Eq != null )
                Eq.ForEach( mobEq => mobEq.Update( mob ) );
        }
    }
}
