using System.Collections.Generic;

namespace Kaerber.MUD.Entities
{
    public enum WearLocation
    {
        NoPickup = -1,
        Inventory = 0,
        Body = 20,
        RightRing = 30,
        RightHand = 40
    };

    public struct WearLocationStrings
    {
        public string Name;
        public string Prefix;

        public static Dictionary<WearLocation, WearLocationStrings> Strings =
            new Dictionary<WearLocation, WearLocationStrings>()
        {
            { WearLocation.NoPickup, new WearLocationStrings { Name = "none", Prefix = "-" } },
            { WearLocation.Inventory, new WearLocationStrings { Name = "none", Prefix = "-" } },
            { WearLocation.Body, new WearLocationStrings { Name = "torso", Prefix = "on the" } },
            { WearLocation.RightRing, new WearLocationStrings { Name = "right finger", Prefix = "on the" } },
            { WearLocation.RightHand, new WearLocationStrings { Name = "right hand", Prefix = "in the" } }
        };

        public static string Format( WearLocation location )
        {
            return ( string.Format( "<{0} {1}>", Strings[location].Prefix, Strings[location].Name ) );
        }
    }
}
