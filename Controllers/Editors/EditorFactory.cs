namespace Kaerber.MUD.Controllers.Editors {
    public class EditorFactory {
        public static EditorFactory Instance = new EditorFactory();

        public void StartEditor( CharacterController pc, string editorName, string id ) {
            switch( editorName ) {
                case "room":
//                    pc.Editor = new RoomEditor( pc );
                    break;

                case "item":
//                    pc.Editor = new ItemEditor( pc );
                    break;

                case "mob":
//                    pc.Editor = new MobEditor( pc );
                    break;

                case "command":
//                    pc.Editor = new CommandEditor( pc );
                    break;

                case "skill":
//                    pc.Editor = new SkillEditor( pc );
                    break;

                case "affect":
//                    pc.Editor = new AffectEditor( pc );
                    break;

                case "aspect":
//                    pc.Editor = new AspectEditor( pc );
                    break;

                default:
                    pc.View.Write( "No such editor.\n" );
                    return;
            }

//            pc.Editor.Start( id );
        }
    }
}
