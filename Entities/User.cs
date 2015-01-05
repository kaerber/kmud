using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class User : IUser, ISerialized {
        public User() { }

        public User( string login, string pass, string email ) : this() {
            Username = login;
            _password = pass;
            Email = email;
        }

        public ISerialized Deserialize( IDictionary<string, object> data ) {
            Username = World.ConvertToType<string>( data["Username"] );
            _password = World.ConvertToType<string>( data["Password"] );
            Email = World.ConvertToType<string>( data["Email"] );
            Characters = World.ConvertToType<List<string>>( data["Players"] );
            LastCharacter = World.ConvertToTypeEx<string>( data, "LastPlayer", string.Empty );
            return( this );
        }

        public IDictionary<string, object> Serialize() {
            return( new Dictionary<string, object>()
                .AddEx( "Username", Username )
                .AddEx( "Password", _password )
                .AddEx( "Players", Characters )
                .AddEx( "Email", Email )
                .AddIf( "LastPlayer", LastCharacter, !string.IsNullOrWhiteSpace( LastCharacter ) )
            );
        }

        public bool CheckPassword( string password ) {
            return _password == password;
        }

        public string Username { get; private set; }
        public string Email { get; private set; }
        public List<string> Characters { get; private set; }
        public string LastCharacter { get; private set; }

        private string _password;

    }
}
