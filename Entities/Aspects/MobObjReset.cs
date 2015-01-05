using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    public class MobObjReset {
        [MudEdit( "Vnum" )]
        public string Vnum { get; set; }

        [MudEdit( "Location" )]
        public WearLocation Location { get; set; }

        public void Update( Character ch ) {
            var item = Item.Create( Vnum );
            if( Location != WearLocation.Inventory )
                ch.Eq.Equip( item );
            else
                ch.Inventory.Add( item );
        }
    }
}
