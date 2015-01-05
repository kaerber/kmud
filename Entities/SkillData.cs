using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class SkillData {
        [MudEdit( "Name of the skill" )]
        public string Name { get; set; }

        [MudEdit("Difficulty of the skill, the higher the value, the more difficult it is to practice.\nSubject to change by auto-balancing features." )]
        public int Rating { get; set; }

        public int Balance { get; set; }
    }
}
