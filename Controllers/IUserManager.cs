using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers {
    public interface IUserManager : IManager<User> {
        bool Exists( string path, string username );
        User Create( string username, string password, string email );
    }
}
