using System.Collections.Generic;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Platform.Managers {
    public class EntitySerializer {
        public static IDictionary<string, object> Serialize( Entity entity ) {
            var data = new Dictionary<string, object> {
                { "Vnum", entity.Id },
                { "Names", entity.Names },
                { "ShortDescr", entity.ShortDescr },
                { "Aspects", entity.Aspects.Serialize() }
            };


            return data;
        }

        public static void Deserialize( dynamic data, Entity entity ) {
            if( data.Aspects != null ) 
                entity.Aspects.Deserialize( data.Aspects );

            entity.Id = data.Vnum;
            entity.Names = data.Names;
            entity.ShortDescr = data.ShortDescr;
        }
    }
}
