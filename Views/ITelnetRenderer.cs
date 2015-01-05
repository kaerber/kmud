using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Views {
    public interface ITelnetRenderer {
        void Render( Item item, Action<string> write );
        void Render( Equipment equipment, Action<string> write );
        void Render( IEnumerable<Item> items, bool longDescription, Action<string> write );
        void Render( Room room, Action<string> write );
        void Render( Character character, Action<string> write );
        void RenderPrompt( Action<string> write );
    }
}
