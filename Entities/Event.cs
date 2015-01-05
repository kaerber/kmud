using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using IronPython.Runtime;


namespace Kaerber.MUD.Entities {
    public class EventArg {
        public EventArg( string name, object value ) {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class Event {
        private readonly string _name;
        private readonly EventReturnMethod _returnMethod;
        private dynamic _returnValue;

        public virtual string Name { get { return _name; } }

        public List<EventArg> Parameters { get; private set; }

        public virtual dynamic this[string parameter] {
            get {
                Contract.Requires( this.Parameters != null );

                var param = Parameters.Find( p => p.Name == parameter );
                return ( param != null ? param.Value : null );
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

                    default:
                        throw new NotSupportedException( "Event return method " + _returnMethod + " is not supported." );
                }
            }
        }


        private Event( string name, EventReturnMethod returnMethod, IEnumerable<EventArg> parameters ) {
            Contract.Requires( parameters != null );

            _name = name;
            _returnMethod = returnMethod;
            Parameters = new List<EventArg>( parameters );

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

                default:
                    throw new NotSupportedException( "Event return method " + _returnMethod + " is not supported." );
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
                              parameters.Select( p => new EventArg( ( string )p.Key, p.Value ) ) );
        }
    }
}
