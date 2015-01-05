using System;
using System.Collections.Generic;
using System.Linq;

using C5;


namespace Kaerber.MUD.Entities.Aspects
{
    public class TimedEventQueue : IntervalHeap<TimedEvent>
    {
        private TimedEventQueue _parent;

        public bool IsAttached { get { return _parent != null; } }

        public TimedEventQueue( TimedEventQueue parent )
        {
            _parent = parent;
        }


        public void Add( long time, Action callback )
        {
            if( _parent != null && ( Count == 0 || time < FindMin().Time ) )
                _parent.Add( time, Run );

            lock( this )
            {
                var e = new TimedEvent( time, callback );
                IPriorityQueueHandle<TimedEvent> handle = null;
                Add( ref handle, e );
                e.Handle = handle;
            }
        }

        public void AddRelative( long relative, Action callback )
        {
            Add( World.Instance.Time + relative, callback );
        }

        public IEnumerable<TimedEvent> Yield( long time )
        {
            var list = new List<TimedEvent>();
            lock( this )
            {
                while( Count > 0 && FindMin().Time - World.TimeStep/2 <= time )
                    list.Add( DeleteMin() );

                if( _parent != null && Count > 0 )
                    _parent.Add( FindMin().Time, Run );
            }

            return ( list );
        }


        public void Run()
        {
            bool workDone;
            do
            {
                workDone = false;
                var queue = Yield( World.Instance.Time );

                foreach( var e in queue )
                {
                    workDone = true;
                    e.Callback();
                }

            } while( workDone );
        }


        public void Attach( TimedEventQueue parent )
        {
            lock( this )
            {
                _parent = parent;
                if( Count > 0 )
                    _parent.Add( FindMin().Time, Run );
            }
        }

        public void Detach()
        {
            lock( this )
            {
                if( _parent == null )
                    return;

                var list = _parent.Where( e => e.Callback == Run ).Select( e => e.Handle ).ToList();
                foreach( var eHandle in list )
                    _parent.Delete( eHandle );

                _parent = null;
            }
        }
    }
}
