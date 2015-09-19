using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaerber.MUD.Entities.Aspects {
    public class MobReset {
        public string Vnum { get; set; }

        public List<MobItemReset> Eq { get; set; }

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
            Eq?.ForEach( mobEq => mobEq.Update( mob ) );
        }


        public static MobReset Deserialize( dynamic data ) {
            Func<dynamic, MobItemReset> deserializeMobItemReset =
                mobItemData => MobItemReset.Deserialize( mobItemData );
            var mobReset = new MobReset {
                Vnum = data.Vnum,
                Eq = new List<MobItemReset>()
            };

            if( data.Eq != null )
                mobReset.Eq = new List<MobItemReset>( 
                    Enumerable.Select( data.Eq, deserializeMobItemReset ) );

            return mobReset;
        }

        public static IDictionary<string, object> Serialize( MobReset mobReset ) {
            return new Dictionary<string, object> {
                ["Vnum"] = mobReset.Vnum,
                ["Eq"] = mobReset.Eq.Select( MobItemReset.Serialize )
            };
        }
    }
}
