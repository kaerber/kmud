using System.IO;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Server.Managers {
    public class UserManager : IUserManager {
        public bool UserExists( string username ) {
            return File.Exists( World.UsersRootPath + username + ".data" );
        }

        public IUser CreateUser( string username, string password, string email ) {
            return new User( username, password, email );
        }

        public IUser LoadUser( string username ) {
            return World.Serializer.Deserialize<User>( 
                File.ReadAllText( World.UsersRootPath + username + ".data" ) );
        }

        public void SaveUser( IUser user ) {
            File.WriteAllText( World.UsersRootPath + user.Username + ".data", 
                World.Serializer.Serialize( this ) );
        }

        public Character LoadCharacter( string name ) {
            return World.Serializer.Deserialize<Character>( File.ReadAllText( World.PlayersRootPath + name + ".data" ) );
        }

        public void SaveCharacter( Character character ) {
            File.WriteAllText( World.PlayersRootPath + character.ShortDescr + ".data", World.Serializer.Serialize( character ) );
        }

        public static readonly UserManager Instance = new UserManager();
    }
}
