using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers {
    public interface IUserManager {
        bool UserExists( string username );

        IUser CreateUser( string username, string password, string email );
        IUser LoadUser( string username );
        void SaveUser( IUser user );

        Character LoadCharacter( string name );
        void SaveCharacter( Character character );
    }
}
