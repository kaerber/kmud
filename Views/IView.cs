using System.Collections.Generic;

namespace Kaerber.MUD.Views {
    public interface IView {
        void Start();

        /// <summary>
        /// A blocking iterator of commands received from Telnet connection and pre-parsed.
        /// </summary>
        IEnumerable<string> Commands();

        void Stop();
    }
}
