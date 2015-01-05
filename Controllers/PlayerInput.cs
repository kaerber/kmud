using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kaerber.MUD.Controllers
{
    public struct PlayerInput
    {
        private static readonly Regex _reArgs = new Regex( "((\\d+[\\*\\.])?([^\\s'\"]+|\".*?\"|'.*?'))", RegexOptions.Compiled );
        private static readonly Regex _reCmd = new Regex( "^(?>\\s*)(?>\\S+)\\s{0,1}", RegexOptions.Compiled );

        public string RawLine;
        public string Command;
        public string RawArguments;
        public List<string> Arguments;

        public static PlayerInput Parse( string input )
        {
            var result = new PlayerInput
                {
                    RawLine = input,
                    Command = _reCmd.Match( input ).Value.Trim(),
                    RawArguments = _reCmd.Replace( input, string.Empty )
                };
            result.Arguments = _reArgs.Matches( result.RawArguments ).Cast<Match>()
                .Select( match => match.Value ).ToList();

            return ( result );
        }
    }
}
