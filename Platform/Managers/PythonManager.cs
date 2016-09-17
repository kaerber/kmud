using System.Collections.Generic;
using IronPython.Hosting;

using Kaerber.MUD.Entities;

using Microsoft.Scripting.Hosting;
using Newtonsoft.Json.Linq;

namespace Kaerber.MUD.Platform.Managers {
    public class PythonManager {
        public ScriptEngine GetEngine() {
            if( _engine != null )
                return _engine;

            var options = new Dictionary<string, object> { ["Debug"] = true };
            _engine = Python.CreateEngine( options );
            var paths = _engine.GetSearchPaths();
            paths.Add( _pythonPath );
            _engine.SetSearchPaths( paths );

            var runtime = _engine.Runtime;
            runtime.LoadAssembly( typeof( Character ).Assembly );
            runtime.LoadAssembly( typeof( JProperty ).Assembly );
            return _engine;
        }


        private ScriptEngine _engine;
        private string _pythonPath = @"E:\Dev\Kaerber.MUD\Python\";

    }
}
