using System.Collections.Generic;

using Kaerber.MUD.Entities.Abilities;

namespace Kaerber.MUD.Entities
{
    public class Specialization : IEventHandler
    {
        private readonly IEnumerable<ICombatAbility> _combatAbilities;
        private readonly IEnumerable<IPassiveAbility> _passiveAbilities;


        public string Id { get; private set; }


        public Specialization( string id, 
            IEnumerable<ICombatAbility> combatAbilities, 
            IEnumerable<IPassiveAbility> passiveAbilities )
        {
            Id = id;
            _combatAbilities = combatAbilities;
            _passiveAbilities = passiveAbilities;
        }


        public void ReceiveEvent( Event e )
        {
            foreach( var ability in _passiveAbilities )
                ability.ReceiveEvent( e );
        }
    }
}
