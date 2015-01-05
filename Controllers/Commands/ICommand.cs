using System;

namespace Kaerber.MUD.Controllers.Commands {
    public interface ICommand : IFormattable {
        string Name { get; }
        string Code { get; set; }
        void Execute( ICharacterController pc, PlayerInput input );
    }

}
