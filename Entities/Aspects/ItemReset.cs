using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    [MudComplexType]
    public class ItemReset {
        public string Vnum { get; set; }

        public void Update( Room host, Action<Item> process = null ) {
            if( !World.Instance.Items.ContainsKey( Vnum ) )
                return;

            if( host.Items.Count( obj => obj.Id == Vnum ) >= 1 )
                return;
            var item = host.Items.Load( Vnum );
            process?.Invoke( item );
        }


        public static ItemReset Deserialize( dynamic data ) {
            return new ItemReset {
                Vnum = data.Vnum
            };
        }

        public static IDictionary<string, object> Serialize( ItemReset itemReset ) {
            return new Dictionary<string, object> {
                ["Vnum"] = itemReset.Vnum
            };
        }
    }
}
