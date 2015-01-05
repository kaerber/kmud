using System.Collections.Generic;

namespace Kaerber.MUD.Entities.Abilities {
    public class KillAbility : ICombatAbility {
        private readonly Character _self;
        private readonly Character _target;

        public KillAbility( Character self, Character target ) {
            _self = self;
            _target = target;
        }

        public Character SelectMainTarget( Character currentTarget ) {
            if( _target != null )
                return _target;

            var foes = _self.GetFoes();
            if( foes.Count == 0 )
                return null;

            if( foes.Count == 1 )
                return foes[0];

            if( currentTarget == null )
                return foes[0];

            return Cycle( foes, currentTarget );
        }

        private static Character Cycle( IList<Character> foes, Character current ) {
            var index = foes.IndexOf( current );
            return index == foes.Count - 1
                ? foes[0]
                : foes[index + 1];
        }

        public void Activate() {} // KillAbility does nothing on activate, it has set target already
    }
}
