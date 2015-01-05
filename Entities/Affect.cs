using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities
{
    public class Affect : ISerialized {
        private static readonly Dictionary<AffectTarget, string> HostParamNames = 
            new Dictionary<AffectTarget, string> {
            { AffectTarget.Character, "affect_ch" },
            { AffectTarget.Item, "affect_item" },
            { AffectTarget.Room, "affect_room" }
        };

        private AffectInfo _info;
        private long _endMoment;
        private Dictionary<string, object> _data;

        public Affect() {}

        public Affect( AffectInfo info ) {
            _info = info;
        }

        #region Load & save
        public ISerialized Deserialize( IDictionary<string, object> data ) {
            var affectName = World.ConvertToType<string>( data["Name"] );
            if( World.Instance.Affects.ContainsKey( affectName ) )
                _info = World.Instance.Affects[affectName];
            else
                throw new EntityException( string.Format( "No affect_data '{0}' found.", affectName ) );

            Duration = World.ConvertToType<long>( data["Duration"] );

            return ( this );
        }

        public IDictionary<string, object> Serialize() {
            return ( new Dictionary<string, object> {
                    { "Name", Name },
                    { "Duration", Duration }
                }
            );
        }
        #endregion

        public string Name {
            get { return ( _info.Name ); }
        }

        public AffectTarget Target {
            get { return ( _info.Target ); }
        }

        public AffectFlags Flags {
            get { return ( _info.Flags ); }
        }

        public long Duration {
            get {
                return ( _endMoment != long.MaxValue
                    ? _endMoment - World.Instance.Time
                    : -1 );
            }
            set {
                _endMoment = value >= 0
                    ? World.Instance.Time + value
                    : long.MaxValue;
            }
        }

        public bool IsCurrent {
            get { return ( _endMoment >= World.Instance.Time ); }
        }

        public Dictionary<string, object> Data {
            get {
                if( _data == null )
                    _data = new Dictionary<string, object>();
                return ( _data );
            }
        }


        public void ReceiveEvent( Event e, object host ) {
            _info.Handlers.Execute(
                e,
                () => new[] { new EventArg( "affect", this ), new EventArg( HostParamNames[Target], host ) }
            );
        }
    }
}
