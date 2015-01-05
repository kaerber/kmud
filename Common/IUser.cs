using System.Collections.Generic;

namespace Kaerber.MUD.Common {
    public interface IUser {
        string Username { get; }
        string Email { get; }
        List<string> Characters { get; } 

        bool CheckPassword( string password );
    }
}
