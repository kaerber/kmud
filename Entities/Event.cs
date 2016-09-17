using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Community.CsharpSqlite;
using IronPython.Runtime;

namespace Kaerber.MUD.Entities {
    public class EventArg {
        public EventArg( string name, dynamic value ) {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public dynamic Value { get; set; }

        public static EventArg Convert<TKey, TValue>( KeyValuePair<TKey, TValue> pair ) {
            return new EventArg( pair.Key.ToString(), pair.Value );
        }
    }

    public class Event {
        private Event( string name, EventReturnMethod returnMethod, IEnumerable<EventArg> parameters ) {
            var parts = name.Split( ':' );
            Name = parts.Last();
            Level = EventLevel.Room;
            if( parts.Length > 1 )
                Level = GetLevel( parts.First() );
            if( parts.Length > 2 )
                Target = parts[1];

            _returnMethod = returnMethod;
            Parameters = new List<EventArg>( parameters ?? new EventArg[0] );

            switch( returnMethod ) {
                case EventReturnMethod.None:
                    break;

                case EventReturnMethod.And:
                    _returnValue = true;
                    break;

                case EventReturnMethod.Or:
                    _returnValue = false;
                    break;

                case EventReturnMethod.List:
                    _returnValue = new List<dynamic>();
                    break;

                case EventReturnMethod.Sum:
                    _returnValue = 0;
                    break;

                case EventReturnMethod.First:
                    _returnValue = null;
                    break;

                default:
                    throw new NotSupportedException( "Event return method " + _returnMethod + " is not supported." );
            }
        }


        public virtual string Name { get; }
        public EventLevel Level { get; }
        public string Target { get; }
        public List<EventArg> Parameters { get; }

        public virtual dynamic this[string parameter] {
            get {
                Contract.Requires( this.Parameters != null );
                return Parameters.Find( p => p.Name == parameter )?.Value;
            }
            set {
                var param = Parameters.FirstOrDefault( p => p.Name == parameter );
                if( param == null )
                    Parameters.Add( new EventArg( parameter, value ) );
                else
                    param.Value = value;
            }
        }

        public dynamic ReturnValue {
            get { return ( _returnValue ); }
            set {
                if( value == null ) return;

                switch( _returnMethod ) {
                    case EventReturnMethod.None:
                        _returnValue = value;
                        break;

                    case EventReturnMethod.And:
                        _returnValue = _returnValue && value;
                        break;

                    case EventReturnMethod.Or:
                        _returnValue = _returnValue || value;
                        break;

                    case EventReturnMethod.List:
                        if( value is List<dynamic> )
                            _returnValue.AddRange( value );
                        else
                            _returnValue.Add( value );
                        break;

                    case EventReturnMethod.Sum:
                        _returnValue += value;
                        break;

                    case EventReturnMethod.First:
                        if( _returnValue == null )
                            _returnValue = value;
                        break;

                    default:
                        throw new NotSupportedException( "Event return method " + _returnMethod + " is not supported." );
                }
            }
        }


        public List<EventArg> MergeParameters( params EventArg[] args ) {
            var result = new List<EventArg>();
            result.AddRange( Parameters );
            result.AddRange( args );

            return result;
        }

        public List<EventArg> MergeParameters( IList<EventArg> args ) {
            var result = new List<EventArg>();
            result.AddRange( Parameters );
            result.AddRange( args );

            return result;
        }


        public Event ChangeToThis( EventArg arg ) {
            var e = new Event(
                ( "_" + Name + "_" ).Replace( "_" + arg.Name + "_", "_this_" ).Trim( '_' ),
                _returnMethod,
                Parameters.Where( p => p.Name != arg.Name ).ToList()
            );

            e.Parameters.Add( new EventArg( "this", arg.Value ) );

            return e;
        }

        public override string ToString() {
            return Name;
        }

        private readonly EventReturnMethod _returnMethod;
        private dynamic _returnValue;

        public static Event Create( string name, 
                                    EventReturnMethod returnMethod = EventReturnMethod.None, 
                                    params EventArg[] parameters ) {
            return new Event( name, returnMethod, parameters );
        }

        public static Event Create( string name, EventReturnMethod returnMethod, PythonTuple parameters ) {
            return new Event( name, 
                              returnMethod, 
                              parameters.Cast<PythonTuple>()
                                        .Select( p => new EventArg( ( string )p[0], p[1] ) ) );
        }

        public static Event Create( string name, EventReturnMethod returnMethod, PythonDictionary parameters ) {
            return new Event( name, 
                              returnMethod, 
                              parameters?.Select( p => new EventArg( ( string )p.Key, p.Value ) ) );
        }

        public static EventLevel GetLevel( string level ) {
            switch( level ) {
                case "entity":
                    return EventLevel.Entity;
                case "room":
                    return EventLevel.Room;
                case "area":
                    return EventLevel.Area;
                case "world":
                    return EventLevel.World;
                default:
                    return EventLevel.Room;
            }
        }
    }
}
