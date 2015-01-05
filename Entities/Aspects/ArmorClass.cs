using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class ArmorClass
    {
        public static ArmorClass operator +( ArmorClass ac1, ArmorClass ac2 ) {
            return ( new ArmorClass
                { Slash = ac1.Slash + ac2.Slash } );
        }

        public ArmorClass() { }

        public ArmorClass( ArmorClass source ) {
            Slash = source.Slash;
        }

        [MudEdit( "Armor class vs slashing weapons" )]
        public int Slash { get; set; }

        public int GetAC() {
            return ( Slash );
        }
    }
}
