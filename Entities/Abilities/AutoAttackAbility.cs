using Kaerber.MUD.Entities.Actions;

using log4net;

namespace Kaerber.MUD.Entities.Abilities {
    public class AutoAttackAbility : IPassiveAbility {
        public virtual void ReceiveEvent( Event e ) {
            if( e.Name == "this_targeted_ch1" || e.Name == "this_is_able_to_perform_autoattack" )
                e["this"].EnqueueAction( "autoattack", new AutoAttackAction() );
        }
    }
}
