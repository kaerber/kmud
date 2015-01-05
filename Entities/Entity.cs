using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

using IronPython.Runtime;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;


namespace Kaerber.MUD.Entities {
    public class Entity : ISerialized, IEventHandler {
        private dynamic _aspectSet;

        protected List<string> names;
        protected bool dirty;

        public World World;
        public Action Dirty;

        public TimedEventQueue UpdateQueue { get; set; }

        public virtual string Id { get; set; }

        [MudEdit( "Names of the entity" )]
        public string Names {
            get { return ( string.Join( " ", names.ToArray() ) ); }
            set {
                names = new List<string>(
                    ( value ?? string.Empty ).ToLower()
                        .Split( ' ' )
                        .Where( str => !string.IsNullOrWhiteSpace( str ) ) );
            }
        }

        [MudEdit( "Short description" )]
        public string ShortDescr { get; set; }

        public AffectSet Affects { get; set; }

        [MudEdit( "Event handlers for this entity" )]
        public HandlerSet Handlers { get; set; }

        [MudEdit( "Aspects", "PythonObject" )]
        public dynamic Aspects {
            get { return ( _aspectSet ); }
            set {
                object ctest = value;
                Contract.Requires( ctest != null );

                _aspectSet = value;
                _aspectSet.Host = this;
            }
        }


        public Entity() {
            World = World.Instance;

            names = new List<string>();
            Affects = new AffectSet();
            Affects.SetHost( this );

            Handlers = new HandlerSet();

            Aspects = AspectFactory.Complex();
            Aspects.Host = this;
        }

        public Entity( string vnum ) : this() {
            Id = vnum;
        }

        public Entity( string vnum, string names, string shortDescr ) : this( vnum ) {
            Names = names;
            ShortDescr = shortDescr;
        }

        public virtual Entity Initialize() {
            return ( this );
        }


        #region Save & load
        public virtual ISerialized Deserialize( IDictionary<string, object> data ) {
            if( data.ContainsKey( "Aspects" ) )
                Aspects.Deserialize( data["Aspects"] );

            Id = World.ConvertToType<string>( data["Vnum"] );
            Names = World.ConvertToTypeEx<string>( data, "Names" );
            ShortDescr = World.ConvertToTypeEx<string>( data, "ShortDescr" );
            
            Affects = new AffectSet(
                World.ConvertToTypeEx( data, "Affects", new List<Affect>() ),
                this );

            Handlers = World.ConvertToTypeEx( data, "Handlers", new HandlerSet() );

            return ( this );
        }

        public virtual IDictionary<string, object> Serialize() {
            var affToSave = Affects.Where( aff => !aff.Flags.HasFlag( AffectFlags.NoSave ) );
            var data = new Dictionary<string, object> {
                    { "Vnum", Id },
                    { "Names", Names },
                    { "ShortDescr", ShortDescr },
                }
                .AddIf( "Affects", affToSave, affToSave.Any() )
                .AddIf( "Handlers", Handlers, Handlers.Count > 0 );

            data.Add( "Aspects", Aspects.Serialize() );

            return ( data );
        }
        #endregion

        #region Events
        public virtual void ReceiveEvent( Event e ) {
            Affects.ReceiveEvent( e );
            Handlers.Execute( e );
            Aspects.ReceiveEvent( e );
        }

        protected Tuple<string, object>[] PrepareHandlerParams( params object[] args ) {
            var argNames = new[] { "arg", "ch", "obj", "room" };
            var argIndices = new int[4];

            var result = new List<Tuple<string, object>>();
            foreach( var arg in args )
            {
                var argType = 0;
                if( arg is Character )
                    argType = 1;
                else if( arg is Item )
                    argType = 2;
                else if( arg is Room )
                    argType = 3;

                result.Add( new Tuple<string, object>( FormArg( argNames[argType], argIndices[argType] ), arg ) );
                argIndices[argType]++;
            }

            return ( result.ToArray() );
        }


        private string FormArg( string argName, int argIndex ) {
            var index = argIndex == 0 ? string.Empty : argIndex.ToString( CultureInfo.InvariantCulture );
            return string.Format( "{0}{1}", argName, index );
        }


        public dynamic Event( Event e ) {
            ReceiveEvent( e );
            return e.ReturnValue;
        }

        public dynamic Event( string name, EventReturnMethod returnMethod, PythonDictionary parameters ) {
            return ( Event( Entities.Event.Create( name, returnMethod, parameters ) ) );
        }

        public dynamic Event( string name, EventReturnMethod returnMethod, params EventArg[] parameters ) {
            return ( Event( Entities.Event.Create( name, returnMethod, parameters ) ) );
        }
        #endregion

        public virtual void BuildRequiredAspects() {}

        public bool MatchNames( string match ) {
            var mx = match.ToLower()
                .Unquote()
                .Split( ' ' )
                .Where( str => !string.IsNullOrWhiteSpace( str ) );
            return ( mx.All( arg => names.Any( name => name.StartsWith( arg ) ) ) );
        }


        protected virtual void OnDirty() {
            if( !dirty )
                lock( this )
                    dirty = true;

            if( Dirty != null )
                Dirty();
        }
    }
}
