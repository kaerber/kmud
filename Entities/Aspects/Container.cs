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


        public Item FindItem( string partialName )
        {
            return ( Items.Find( item => item.MatchNames( partialName ) ) );
        }
    }
}
