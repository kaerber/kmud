using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Newtonsoft.Json;

namespace Kaerber.MUD.Platform.Managers {
    public class UserManager : IUserManager {
        public UserManager( string root, IManager<Character> characterManager ) {
            _root = root;
            _characterManager = characterManager;
        }

        public bool Exists( string path, string username ) {
            return File.Exists( FormPath( path, username ) );
        }

        public IList<string> List( string path ) {
            throw new System.NotImplementedException();
        }

        public User Create( string username, string password, string email ) {
            return new User( username, password, email );
        }

        public User Load( string path, string username ) {
            var user = Deserialize( JsonConvert.DeserializeObject( 
                File.ReadAllText( FormPath( path, username ) ) ) );
            var userpath = Path.Combine( _root, path, username );
            user.Characters = _characterManager.List( userpath )
                                               .ToList();
            return user;
        }

        public void Save( string path, User user ) {
            var userpath = FormPath( path, user.Username );
            var userdirectory = Path.GetDirectoryName( userpath );
            if( !Directory.Exists( userdirectory ) )
                Directory.CreateDirectory( userdirectory );
            File.WriteAllText( userpath, 
                                JsonConvert.SerializeObject( Serialize( user ) ) );
        }

        private string FormPath( string path, string name ) {
            return Path.Combine( _root, path, name, "user.json" );
        }


        public static User Deserialize( dynamic data ) {
            var user = new User( ( string )data.Username, ( string )data.Password, ( string )data.Email ) { 
                LastCharacter = data.LastPlayer ?? string.Empty
            };
            return user;
        }

        public static IDictionary<string, object> Serialize( User user ) {
            return ( new Dictionary<string, object>()
                .AddEx( "Username", user.Username )
                .AddEx( "Password", user.Password )
                .AddEx( "Players", user.Characters )
                .AddEx( "Email", user.Email )
                .AddIf( "LastPlayer", user.LastCharacter, !string.IsNullOrWhiteSpace( user.LastCharacter ) )
            );
        }

        private readonly string _root;
        private readonly IManager<Character> _characterManager;
    }
}
