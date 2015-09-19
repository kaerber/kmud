using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class User : IUser {
        public User() { }

        public User( string login, string pass, string email ) : this() {
            Username = login;
            Password = pass;
            Email = email;
            Characters = new List<string>();
        }

        public bool CheckPassword( string password ) {
            return Password == password;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Characters { get; set; }
        public string LastCharacter { get; set; }

        public string Password { get; }
    }
}
