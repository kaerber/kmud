using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IronPython.Runtime.Exceptions;

using Kaerber.MUD.Common;

using Microsoft.Scripting.Hosting;

using IronPython.Hosting;

namespace Kaerber.MUD.Entities {
    public class MLFunction {
        static MLFunction() {
            _engine = Python.CreateEngine();
            var searchPaths = _engine.GetSearchPaths();
            searchPaths.Add( World.LibPath );
            searchPaths.Add( World.MlLibPath );
            _engine.SetSearchPaths( searchPaths );

            LoadAssemblies(
                Assembly.GetAssembly( typeof( Character ) ),
                Assembly.GetAssembly( typeof( IEnumerable<string> ) ),
                Assembly.GetAssembly( typeof( Enumerable ) ) );
        }

        public string Code {
            get { return _code; }
            set {
                _compiledCode = null;
                _code = value;
            }
        }

        public dynamic Execute( params EventArg[] args ) {
            return Execute( args.ToDictionary( a => a.Name, a => a.Value ) );
        }

        public dynamic Execute( List<EventArg> args ) {
            return Execute( args.ToDictionary( a => a.Name, a => a.Value ) );
        }

        public dynamic Execute( Dictionary<string, dynamic> args ) {
            if( _compiledCode == null )
                _compiledCode = Compile();
            var scope = _engine.CreateScope();
            foreach( var arg in args )
                scope.SetVariable( arg.Key, arg.Value );
            scope.SetVariable( "world", World.Instance );
            scope.SetVariable( "handler_data", _data );

            try {
                _compiledCode.Execute( scope );
            }
            catch( SystemExitException ) { } // Exit from function

            dynamic returnValue;
            return scope.TryGetVariable( "ret_val", out returnValue )
                ? returnValue
                : null;
        }

        private CompiledCode Compile() {
            var source = _engine.CreateScriptSourceFromString( _code );
            return source.Compile();
        }

        public static void LoadAssemblies( params Assembly[] assemblies ) {
            _engine.Runtime.LoadAssembly( Assembly.GetAssembly( typeof( IronPython.Modules.ArrayModule ) ) );
            assemblies.ToList().ForEach( assembly => _engine.Runtime.LoadAssembly( assembly ) );
        }

        public static dynamic Eval( string code ) {
            return new MLFunction { Code = code }.Execute();
        }


        public static MLFunction Deserialize( dynamic data ) {
            return new MLFunction {
                Code = data.Code
            };
        }

        public static IDictionary<string, object> Serialize( MLFunction mlFunction ) {
            return new Dictionary<string, object>()
                .AddEx( "Code", mlFunction.Code );
        }

        private string _code;
        private CompiledCode _compiledCode;
        private readonly Dictionary<string, dynamic> _data = new Dictionary<string, dynamic>();

        private static readonly ScriptEngine _engine;
    }
}
