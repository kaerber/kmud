using System.Collections.Generic;

namespace Kaerber.MUD.Common {
    public interface ISerialized {
        ISerialized Deserialize( IDictionary<string, object> data );
        IDictionary<string, object> Serialize();
    }
}
