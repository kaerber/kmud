using System;

using C5;

namespace Kaerber.MUD.Entities.Aspects {
    public class TimedEvent : IComparable<TimedEvent> {
        public long Time { get; private set; }
        public Action Callback { get; private set; }

        public IPriorityQueueHandle<TimedEvent> Handle { get; set; }

        public TimedEvent( long time, Action callback ) {
            Time = time;
            Callback = callback;
        }

        public int CompareTo( TimedEvent other ) {
            return Time.CompareTo( other.Time );
        }
    }
}
