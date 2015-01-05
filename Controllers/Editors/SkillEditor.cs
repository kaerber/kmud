using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Editors {
    public class SkillEditor : BaseEditor<SkillData> {
        private const string LastEditedKey = "skill_edit_last_cmd";

        public SkillEditor( CharacterController pc ) : base( pc ) {}

        public override string Name { get { return "skill"; } }

        public override SkillData Value { get { return Current; } }

        public override SkillData Current {
            get { return base.Current; }
            set {
                base.Current = value;
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }

        protected override SkillData LastEdited {
            get {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    return null;
                if( !World.Instance.Skills.ContainsKey( pc.Model.Data[LastEditedKey] ) )
                    return null;
                return World.Instance.Skills[pc.Model.Data[LastEditedKey]];
            }
            set {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    pc.Model.Data.Add( LastEditedKey, string.Empty );
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }


        protected override IEnumerable<SkillData> List {
            get { return World.Instance.Skills.Values; }
        }

        public override void PrintList() {
            foreach( var skill in List )
                pc.View.WriteFormat( "{0}\n", skill.Name );
        }


        public override SkillData Create( string skillName ) {
            if( string.IsNullOrWhiteSpace( skillName ) ) {
                pc.View.Write( "Invalid skill name.\n" );
                return null;
            }

            var skill = new SkillData { Name = skillName, Rating = 1, Balance = 0 };
            World.Instance.Skills.Add( skillName, skill );
            Current = skill;
            pc.View.WriteFormat( "{0} '{1}' created.\n", Name, skillName );
            return Current;
        }


        public override void Delete() {
            pc.View.Write( "This operation is not implemented.\n" );
        }


        public override void ChangeTo( string skillName ) {
            SkillData target = null;
            if( !string.IsNullOrWhiteSpace( skillName ) ) {
                target = List.FirstOrDefault( skill => skill.Name.StartsWith( skillName,
                     StringComparison.CurrentCultureIgnoreCase ) );
            }

            if( target != null ) {
                Current = target;
                LastEdited = target;
            }
            else {
                if( !string.IsNullOrWhiteSpace( skillName ) ) {
                    pc.View.WriteFormat( "{0} [{1}] not found.\n", Name, skillName );
                }
                if( LastEdited != null ) {
                    Current = LastEdited;
                }
            }

            if( Current != null ) {
                pc.View.WriteFormat( "Now editing {0} '{1}'.\n", Name, Current.Name );
            }
        }

        public override void Save() {
            var area = World.Instance;
            area.SetDirty();
            area.Save();
            pc.View.WriteFormat( "Area {0} saved.\n", area.Id );
        }
    }
}
