using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands.Editor;
using Kaerber.MUD.Views;

using CommandSet = System.Collections.Generic.Dictionary<string, Kaerber.MUD.Controllers.Commands.ICommand>;
using Exit = Kaerber.MUD.Controllers.Commands.Editor.Exit;

namespace Kaerber.MUD.Controllers {
    public class EditorController<T> : IEditorController<T> {
        public EditorController( IEditorView<T> view, IManager<T> manager, T entity ) {
            _view = view;
            _manager = manager;
            _entity = entity;
        }

        public void Save() {
            _manager.Save( string.Empty, _entity );
        }

        public void List() {
            _view.List();
        }

        private readonly IManager<T> _manager;
        private readonly IEditorView<T> _view;
        private readonly T _entity;

        private static CommandSet _commandSet = new CommandSet {
            { "create", new Create() },
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
