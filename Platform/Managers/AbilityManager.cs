using System.IO;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Platform.Managers {
    public class AbilityManager {
        public AbilityManager( PythonManager pythonManager ) {
            _pythonManager = pythonManager;
        }

        public IAbility Movement() {
            var engine = _pythonManager.GetEngine();
            var script = engine.CreateScriptSourceFromFile( Path.Combine( _pythonPath, "movement.py" ) );
            var code = script.Compile();
            var scope = engine.CreateScope();
            code.Execute( scope );

            return scope.GetVariable( "result" );
        }

        private readonly PythonManager _pythonManager;
        private string _pythonPath = @"E:\Dev\Kaerber.MUD\Python\abilities";
    }
}
