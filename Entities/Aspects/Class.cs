using Kaerber.MUD.Entities.Abilities;

namespace Kaerber.MUD.Entities.Aspects {
    public class Class : EventTarget {

        public Class( string name, dynamic stats ) {
            Id = name;
            AutoAttack = new AutoAttackAbility();
            Stats = stats;
        }

        public string Id { get; }

        public dynamic Stats { get; }
        public AutoAttackAbility AutoAttack { get; set; }

        public override void ReceiveEvent( Event e ) {
            Stats.ReceiveEvent( e );
            AutoAttack.ReceiveEvent( e );
        }
    }
}
