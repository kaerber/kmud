using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Tests.Acceptance
{
    public class EventLogger : IAspect
    {
        public List<Event> Log = new List<Event>();

        public IAspect Clone() {
            throw new NotImplementedException();
        }


        public Entity Host { get; set; }

        public IDictionary<string, object> Serialize() {
            throw new NotImplementedException();
        }


        public IAspect Deserialize( IDictionary<string, object> data ) {
            throw new NotImplementedException();
        }


        public void ReceiveEvent( Event e ) {
            Log.Add( e );
        }
    }
}
