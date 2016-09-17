using System.Collections.Generic;
using System.Linq;
using IronPython.Runtime;
using static Kaerber.MUD.Entities.Event;

namespace Kaerber.MUD.Entities {
    public interface IEventSource {
        void SendEvent( Event e );
    }

    public static class EventSourceApi {
        public static Event DoEvent( this IEventSource self,
                                     string name,
                                     EventReturnMethod returnMethod,
                                     IList<EventArg> args ) {
            args = ( args ?? new List<EventArg>() )
                .Concat( new[] {new EventArg( "ch", self )} )
                .ToList();
            var doEvent = Create( name, returnMethod, args.ToArray() );

            self.SendEvent( doEvent );

            return doEvent;
        }

        public static Event DoEvent( this IEventSource self,
                                     string name,
                                     EventReturnMethod returnMethod,
                                     PythonDictionary args = null ) {
            args = args ?? new PythonDictionary();
            var argList = args.Select( EventArg.Convert )
                              .ToList();
            return self.DoEvent( name, returnMethod, argList );
        }

        public static bool Can( this IEventSource self, string action, PythonDictionary args = null ) {
            var canEvent = self.DoEvent( "ch_can_" + action, EventReturnMethod.And, args );
            return canEvent.ReturnValue;
        }

        public static bool Can( this IEventSource self, string action, params EventArg[] args ) {
            var @event = self.DoEvent( "ch_can_" + action, EventReturnMethod.And, args );
            return @event.ReturnValue;
        }

        public static void Is( this IEventSource self, string action, PythonDictionary args = null ) {
            self.DoEvent( "ch_is_" + action, EventReturnMethod.None, args );
        }

        public static void Has( this IEventSource self, string action, params EventArg[] args ) {
            self.DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }

        public static void Has( this IEventSource self, string action, PythonDictionary args = null ) {
            self.DoEvent( "ch_" + action, EventReturnMethod.None, args );
        }


    }
}
