using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;

namespace Kaerber.MUD.Platform.Managers {
    public class AbilityManager : IManager<IAbility> {
        public AbilityManager( PythonManager pythonManager, string root ) {
            _pythonManager = pythonManager;
            _root = root;
            _engine = _pythonManager.GetEngine();
            _abilityConstructors = new Dictionary<string, Func<IAbility>>();
            _actions = new Dictionary<string, IAction>();
        }

        public IList<string> List( string path ) {
            var abilitypath = Path.Combine( path, "abilities" );
            return Directory.GetFiles( abilitypath )
                            .Select( Path.GetFileNameWithoutExtension )
                            .ToList();
        }

        public IAbility Load( string path, string name ) {
            var ability = Get( name );
            var filepath = Path.Combine( path, "abilities", name + ".json" );
            var state = JsonConvert.DeserializeObject( File.ReadAllText( filepath ) );
            ability.SetState( state );

            return ability;
        }

        public void Save( string path, IAbility entity ) {
            throw new NotImplementedException();
        }

        public IAbility Get( string ability ) {
            var result = LoadAbility( ability );
            result.Actions = ListActions( ability )
                .ToDictionary( action => action, action => LoadAction( ability, action ) );
            return result;
        }

        private IAbility LoadAbility( string ability ) {
            if( _abilityConstructors.ContainsKey( ability ) )
                return _abilityConstructors[ability]();

            var script = _engine.CreateScriptSourceFromFile( 
                Path.Combine( AbilityDirectory( ability ), "_.py" ) );
            var code = script.Compile();
            var scope = _engine.CreateScope();
            Func<IAbility> constructor = () => {
                code.Execute( scope );
                return scope.GetVariable( "result" );
            };
            _abilityConstructors.Add( ability, constructor );

            return constructor();
        }

        private IEnumerable<string> ListActions( string ability ) {
            return Directory.GetFiles( AbilityDirectory( ability ) )
                            .Select( Path.GetFileNameWithoutExtension )
                            .Except( new[] { "_", "__init__" } );
        }

        private IAction LoadAction( string ability, string action ) {
            if( _actions.ContainsKey( action ) )
                return _actions[action];

            var script = _engine.CreateScriptSourceFromFile(
                Path.Combine( AbilityDirectory( ability ), action + ".py" ) );
            var code = script.Compile();
            var scope = _engine.CreateScope();
            code.Execute( scope );
            var result = scope.GetVariable( "result" );
            _actions.Add( action, result );

            return result;

        }

        private string AbilityDirectory( string ability ) {
            return Path.Combine( _root, ability );
        }

        private readonly PythonManager _pythonManager;
        private readonly string _root;

        private readonly ScriptEngine _engine;
        private readonly Dictionary<string, Func<IAbility>> _abilityConstructors;
        private readonly Dictionary<string, IAction> _actions;
    }
}
