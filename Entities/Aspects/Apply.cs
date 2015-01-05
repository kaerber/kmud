using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class Apply
    {
        public static Apply operator +( Apply apply1, Apply apply2 )
        {
            return ( new Apply
                { AC = apply1.AC + apply2.AC,
                  Armor = apply1.Armor + apply2.Armor,
                  HP = apply1.HP + apply2.HP,
                  Mana = apply1.Mana + apply2.Mana,
                  HitRoll = apply1.HitRoll + apply2.HitRoll } );
        }

        public Apply()
        {
            AC = new ArmorClass();
        }

        public Apply( Apply source )
        {
            HP = source.HP;
            AC = new ArmorClass( source.AC );
            Armor = source.Armor;
            HitRoll = source.HitRoll;
        }

        [MudEdit( "Health" )]
        public int HP { get; set; }

        [MudEdit( "Mana" )]
        public int Mana { get; set; }

        
        private ArmorClass AC { get; set; }

        [MudEdit( "Armor" )]
        public int Armor { get; set; }

        [MudEdit( "Bonus to the chance to hit enemy" )]
        public int HitRoll { get; set; }
    }
}
