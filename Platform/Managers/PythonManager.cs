using IronPython.Hosting;

using Kaerber.MUD.Entities;

using Microsoft.Scripting.Hosting;

namespace Kaerber.MUD.Platform.Managers {
    public class PythonManager {
        public ScriptEngine GetEngine() {
            if( _engine != null )
                return _engine;

            _engine = Python.CreateEngine();
            var paths = _engine.GetSearchPaths();
            paths.Add( _pythonPath );
            _engine.SetSearchPaths( paths );

            var runtime = _engine.Runtime;
            runtime.LoadAssembly( typeof( CharacterCore ).Assembly );
            return _engine;
        }


        private ScriptEngine _engine;
        private string _pythonPath = @"E:\Dev\Kaerber.MUD\Python\";

    }
}
