using System;
using System.Collections.Generic;
using System.IO;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Server.Managers {
    public class CharacterManager : IManager<Character> {
        public Character Get( string name ) {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> List() {
            throw new NotImplementedException();
        }

        public void Save( Character entity ) {
            File.WriteAllText( World.PlayersRootPath + entity.ShortDescr + ".data",
                               World.Serializer.Serialize( entity ) );
        }
    }
}
