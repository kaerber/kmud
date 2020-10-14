using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Newtonsoft.Json;

namespace Kaerber.MUD.Platform.Managers {
    public class AbilityManager : IManager<IAbility> {
        public AbilityManager() {
            _abilityConstructors = new Dictionary<string, Func<IAbility>>();
        }

        public IList<string> List( string path ) {
            return _abilityConstructors.Keys.ToList();
        }

        public IAbility Load( string path, string name ) {
            var ability = Get( name );
            var filepath = Path.Combine( path, "abilities", name + ".json" );
            var state = JsonConvert.DeserializeObject( File.ReadAllText( filepath ) );
            ability.SetState( state );

            return ability;
        }

        public void Save( string path, IAbility entity ) {
            throw new NotImplementedException();
        }

        public IAbility Get( string ability ) {
            Debug.Assert( _abilityConstructors.ContainsKey( ability ) );
            var result = _abilityConstructors[ability]();
            return result;
        }

        private readonly Dictionary<string, Func<IAbility>> _abilityConstructors;
    }
}
