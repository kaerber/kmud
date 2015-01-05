using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    [Serializable]
	public class SkillSet : Dictionary<string, Skill>, ISerialized {
	    public Character Host { get; set; }

        public ISerialized Deserialize( IDictionary<string, object> data ) {
            data.ToList().ForEach( pair => {
                    Add( pair.Key, World.ConvertToType<Skill>( pair.Value ) );
                    this[pair.Key].SkillImproved += SkillImproved;
                } );
            return ( this );
        }

        public IDictionary<string, object> Serialize() {
            return( this.ToDictionary(
                pair => pair.Key,
                pair => ( object )pair.Value ) );
        }

        public void SkillImproved( Skill skill, int value ) {
            Host.ReceiveEvent(
                Event.Create( "ch_improves_skill", EventReturnMethod.None,
                    new EventArg( "ch", Host ), new EventArg( "skill", skill.Name )
                )
            );
        }
    }
}
