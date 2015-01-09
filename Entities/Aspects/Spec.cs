using Kaerber.MUD.Entities.Abilities;

namespace Kaerber.MUD.Entities.Aspects {
    public class Spec : IEventHandler {
        public AutoAttackAbility AutoAttack;

        public Spec( string name ) {
            Id = name;
            AutoAttack = new AutoAttackAbility();
        }

        public string Id { get; private set; }

        public void ReceiveEvent( Event e ) {
            AutoAttack.ReceiveEvent( e );
        }
    }
}
