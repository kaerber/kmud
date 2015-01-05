using System;

namespace Kaerber.MUD.Controllers.Commands.Editor
{
    public class List : ICommand
    {
        public string Name {
            get { return "List"; }
            set { throw new NotImplementedException(); }
        }

        public string Code {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input )
        {
            //( ( CharacterController )pc ).Editor.PrintList();
            pc.View.Write( "\n" );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
