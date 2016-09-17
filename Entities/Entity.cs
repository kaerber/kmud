using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities.Aspects;


namespace Kaerber.MUD.Entities {
    public class Entity : EventTarget {
        public Entity() {
            World = World.Instance;
            names = new List<string>();

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

        public override string Level => "entity";

        public TimedEventQueue UpdateQueue { get; set; }

        public virtual string Id { get; set; }

        public string Names {
            get { return string.Join( " ", names.ToArray() ); }
            set {
                names = new List<string>(
                    ( value ?? string.Empty ).ToLower()
                        .Split( ' ' )
                        .Where( str => !string.IsNullOrWhiteSpace( str ) ) );
            }
        }

        public string ShortDescr { get; set; }

        public dynamic Aspects {
            get { return _aspectSet; }
            set {
                _aspectSet = value;
                _aspectSet.Host = this;
            }
        }


        public virtual Entity Initialize() {
            return this;
        }

        public override void ReceiveEvent( Event e ) {
            Aspects.ReceiveEvent( e );
        }


        public bool MatchNames( string match ) {
            var mx = match.ToLower()
                .Unquote()
                .Split( ' ' )
                .Where( str => !string.IsNullOrWhiteSpace( str ) );
            return mx.All( arg => names.Any( name => name.StartsWith( arg ) ) );
        }

        public World World;

        protected List<string> names;

        private dynamic _aspectSet;
    }
}
