using System;
using System.Collections.Generic;
using System.Globalization;
using IronPython.Runtime;

namespace Kaerber.MUD.Entities {
    public class EventTarget : IEventTarget {
        public Action<Event> EventSink;

        public virtual string Level => "event-target";

        public virtual void ReceiveEvent( Event e ) {
        }

        public virtual void SendEvent( Event e ) {
            if( e.Target == Level )
                ReceiveEvent( e );
            else
                EventSink?.Invoke( e );
        }

        public dynamic Event( Event e ) {
            ReceiveEvent( e );
            return e.ReturnValue;
        }

        public dynamic Event( string name,
                              EventReturnMethod returnMethod = EventReturnMethod.None,
                              PythonDictionary parameters = null ) {
            return Event( Entities.Event.Create( name, returnMethod, parameters ) );
        }

        public dynamic Event( string name,
                              EventReturnMethod returnMethod = EventReturnMethod.None,
                              params EventArg[] parameters ) {
            return Event( Entities.Event.Create( name, returnMethod, parameters ) );
        }

        protected Tuple<string, object>[] PrepareHandlerParams( params object[] args ) {
            var argNames = new[] { "arg", "ch", "obj", "room" };
            var argIndices = new int[4];

            var result = new List<Tuple<string, object>>();
            foreach( var arg in args ) {
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

            return result.ToArray();
        }

        private static string FormArg( string argName, int argIndex ) {
            var index = argIndex == 0 ? string.Empty : argIndex.ToString( CultureInfo.InvariantCulture );
            return $"{argName}{index}";
        }
    }
}
