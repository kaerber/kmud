using System.Collections.Generic;

namespace Kaerber.MUD.Entities.Aspects
{
    public interface IAspect
    {
        IAspect Clone();

        Entity Host { get; set; }

        IDictionary<string, object> Serialize();
        IAspect Deserialize( IDictionary<string, object> data );
        void ReceiveEvent( Event e );
    }
}
