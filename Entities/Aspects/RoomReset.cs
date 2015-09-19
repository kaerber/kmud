using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class RoomReset {
        public Room Host { get; set; }

        public List<MobReset> MobResets { get; set; }

        public List<ItemReset> ItemResets { get; set; }


        public void Update() {
            MobResets?.ForEach( mr => mr.Update( Host ) );
            ItemResets?.ForEach( or => or.Update( Host ) );
        }


        public static RoomReset Deserialize( dynamic data ) {
            var roomReset = new RoomReset();

            if( data.MobResets != null ) {
                Func<dynamic, MobReset> deserializeMobReset =
                    mobResetData => MobReset.Deserialize( mobResetData );
                roomReset.MobResets = new List<MobReset>(
                    Enumerable.Select( data.MobResets, deserializeMobReset ) );
            }

            if( data.ItemResets != null ) {
                Func<dynamic, ItemReset> deserializeItemReset =
                    itemResetData => ItemReset.Deserialize( itemResetData );
                roomReset.ItemResets = new List<ItemReset>(
                    Enumerable.Select( data.ItemResets, deserializeItemReset ) );
            }

            return roomReset;
        }

        public static IDictionary<string, object> Serialize( RoomReset roomReset ) {
            var data = new Dictionary<string, object>();
            if( roomReset.MobResets != null && roomReset.MobResets.Count > 0 )
                data.Add( "MobResets", roomReset.MobResets.Select( MobReset.Serialize ) );
            if( roomReset.ItemResets != null && roomReset.ItemResets.Count > 0 )
                data.Add( "ItemResets", roomReset.ItemResets.Select( ItemReset.Serialize ) );

            return data;
        }
    }
}
