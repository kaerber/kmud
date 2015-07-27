using System;
using System.Collections.Generic;

namespace Kaerber.MUD.Entities {
    public interface IAbility {
        string Name { get; }
        Action<Event> EventSink { set; }

        void ReceiveEvent( Event e );

        IList<IAction> Actions { get; }
    }
}
