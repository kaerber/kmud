using System;
using System.Collections.Generic;
using System.IO;

using Kaerber.MUD.Entities;
using Microsoft.Scripting.Hosting;

namespace Kaerber.MUD.Platform.Managers {
    public class AbilityManager {
        public AbilityManager( PythonManager pythonManager, string path ) {
            _pythonManager = pythonManager;
            _path = path;
            _engine = _pythonManager.GetEngine();
            _constructors = new Dictionary<string, Func<IAbility>>();
        }

        public IAbility Movement() {
            var script = _engine.CreateScriptSourceFromFile( Path.Combine( _path, "movement.py" ) );
            var code = script.Compile();
            var scope = _engine.CreateScope();
            code.Execute( scope );

            return scope.GetVariable( "result" );
        }

        public IAbility Get( string name ) {
            if( _constructors.ContainsKey( name ) )
                return _constructors[name]();

            var script = _engine.CreateScriptSourceFromFile( Path.Combine( _path, name + ".py" ) );
            var code = script.Compile();
            var scope = _engine.CreateScope();
            Func<IAbility> constructor = () => {
                code.Execute( scope );
                return scope.GetVariable( "result" );
            };
            _constructors.Add( name, constructor );

            return constructor();
        }

        private readonly PythonManager _pythonManager;
        private readonly string _path;

        private readonly ScriptEngine _engine;
        private readonly Dictionary<string, Func<IAbility>> _constructors;

    }
}
