using System;
using System.Collections.Generic;

namespace Kaerber.MUD.Entities {
    public interface IAbility {
        Action<Event> EventSink { set; }

        void ReceiveEvent( Event e );
        dynamic GetState();
        void SetState( dynamic state );

        IDictionary<string, IAction> Actions { get; set; }
    }
}
