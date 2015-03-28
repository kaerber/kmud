using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Views {
    public interface ITelnetInputParser {
        string Parse( string input, Character ch );
    }
}
