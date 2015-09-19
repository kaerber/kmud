using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    [MudComplexType]
    public class Container {
        public List<Item> Items;

        public Container() {
            Items = new List<Item>();
        }

        public Container( Container template ) : this() {
        }


        public static Container Deserialize( dynamic data ) {
            return new Container();
        }

        public static IDictionary<string, object> Serialize( Container container ) {
            return new Dictionary<string, object>();
        }
    }
}
