using System;
using System.Collections.Generic;
using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    public class MobItemReset {
        public string Vnum { get; set; }

        public WearLocation Location { get; set; }

        public void Update( Character ch ) {
            var item = Item.Create( Vnum );
            if( Location != WearLocation.Inventory )
                ch.Eq.Equip( item );
            else
                ch.Inventory.Add( item );
        }


        public static MobItemReset Deserialize( dynamic data ) {
            return new MobItemReset {
                Vnum = data.Vnum,
                Location = ( WearLocation )Enum.Parse( typeof( WearLocation ), ( string )data.Location )
            };
        }

        public static IDictionary<string, object> Serialize( MobItemReset mobItemReset ) {
            return new Dictionary<string, object> {
                ["Vnum"] = mobItemReset.Vnum,
                ["Location"] = mobItemReset.Location
            };
        }
    }
}
