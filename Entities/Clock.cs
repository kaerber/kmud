using System;

namespace Kaerber.MUD.Entities {
    public class Clock {
        public const int TimeStep = 10;
        public const int TimeBase = 1000;
        public const int TimeRound = TimeBase*3;
        public const int RoundsInHour = 6;
        public const int TimeExtra = TimeBase*2;
        public const int TimeHour = TimeRound*RoundsInHour + TimeExtra;

        public Clock( long ticks ) {
            _ticks = ticks;
        }

        public event Action<long> Update;
        public event Action Round;
        public event Action Hour;


        public long Time { get; private set; }

        public void Pulse( long ticks ) {
            Time += ( ticks - _ticks )/10000;
            _ticks = ticks;

            Update?.Invoke( Time );

            // tick
            if( Time/TimeHour > _hour ) {
                _hour++;
                _round = 0;
                Hour?.Invoke();
            }

            // round
            if( ( Time % TimeHour ) / TimeRound > _round ) {
                _round++;
                Round?.Invoke();
            }
        }

        private long _ticks;
        private long _round;
        private long _hour;
    }
}
