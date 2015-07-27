using System.Collections.Generic;
using System.IO;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Platform.Managers {
    public class UserManager : IUserManager {
        public UserManager( string root ) {
            _root = root;
        }

        public bool Exists( string path, string username ) {
            return File.Exists( FormPath( path, username ) );
        }

        public IList<string> List( string path ) {
            throw new System.NotImplementedException();
        }

        public IUser Create( string username, string password, string email ) {
            return new User( username, password, email );
        }

        public IUser Load( string path, string username ) {
            return World.Serializer.Deserialize<User>(
                File.ReadAllText( FormPath( path, username ) ) );
        }

        public void Save( string path, IUser user ) {
            File.WriteAllText( FormPath( path, user.Username ),
                               World.Serializer.Serialize( this ) );
        }

        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, name, "user.data" );
        }

        private readonly string _root;
    }
}
