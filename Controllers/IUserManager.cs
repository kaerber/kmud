using Kaerber.MUD.Common;

namespace Kaerber.MUD.Controllers {
    public interface IUserManager : IManager<IUser> {
        bool Exists( string path, string username );
        IUser Create( string username, string password, string email );
    }
}
