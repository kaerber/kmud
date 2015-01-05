using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Controllers.Commands.Editor;

namespace Kaerber.MUD.Controllers.Editors
{
    public abstract class BaseEditor<T> : IEditor<T> {
        protected CharacterController pc;

        protected BaseEditor( CharacterController pc ) {
            this.pc = pc;
        }

        public virtual void Start( string id ) {
            pc.View.WriteFormat( "Switching to {0}_edit.\n", Name );

            ChangeTo( id );
        }

        protected virtual Dictionary<string, ICommand> CommandSet {
            get {
                return new Dictionary<string, ICommand> {
                    { "create", new Create() },
                    { "delete", new Delete() },
                    { "look", new Look() },
                    { "ls", new List() },
                    { "cd", new ChangeDirectory() },
                    { "exit", new Exit() },
                    { "done", new Exit() },
                    { "set", new EditField() },
                    { "default", new EditField() }
                };
            }
        }

        public abstract string Name { get; }

        public virtual T Current { get; set; }
        protected abstract T LastEdited { get; set; }
        public abstract T Value { get; }
        protected abstract IEnumerable<T> List { get; }
        public virtual void PrintList() {
            foreach( var name in List.Cast<string>() )
                pc.View.Write( string.Format( "{0}\n", name ) );
        }

        public abstract void ChangeTo( string id );
        public abstract T Create( string vnum );
        public abstract void Delete();
        public abstract void Save();
    }
}
