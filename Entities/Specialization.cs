using System.Collections.Generic;

using Kaerber.MUD.Entities.Abilities;

namespace Kaerber.MUD.Entities {
    public class Specialization : EventTarget {
        private readonly IEnumerable<IPassiveAbility> _passiveAbilities;


        public string Id { get; private set; }


        public Specialization( string id, IEnumerable<IPassiveAbility> passiveAbilities ) {
            Id = id;
            _passiveAbilities = passiveAbilities;
        }


        public override void ReceiveEvent( Event e ) {
            foreach( var ability in _passiveAbilities )
                ability.ReceiveEvent( e );
        }
    }
}
