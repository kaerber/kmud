using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities.Actions {
    public class AutoAttackAction : CharacterAction {
        public override int SharedCooldown {
            get { return Clock.TimeRound; }
        }

        public override void Execute() {
            if( Character.Target == null )
                return;
            Character.MakeAttack( AspectFactory.Attack() );
        }
    }
}
