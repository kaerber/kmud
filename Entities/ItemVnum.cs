using System;

using log4net;

namespace Kaerber.MUD.Entities {
    public class ItemVnum {
        public static bool operator == ( ItemVnum vnum1, ItemVnum vnum2 ) {
            if( vnum1 == null && vnum2 == null )
                return true;
            if( vnum1 == null || vnum2 == null )
                return false;
            return ( vnum1._vnum.Equals( vnum2._vnum, StringComparison.InvariantCultureIgnoreCase ) );
        }

        public static bool operator != ( ItemVnum vnum1, ItemVnum vnum2 ) {
            return ( !( vnum1 == vnum2 ) );
        }

        public override bool Equals( object obj ) {
            var vnum = obj as ItemVnum;
            return vnum != null && this == vnum;
        }

        public override int GetHashCode() {
            return _vnum.GetHashCode();
        }

        public ItemVnum() {}

        private ItemVnum( string vnum ) {
            _vnum = vnum;
        }

        public override string ToString() {
            return _vnum;
        }

        public bool IsEmpty { get { return string.IsNullOrWhiteSpace( _vnum ); } }

        private readonly string _vnum;

        public static ItemVnum FromString( string vnum, bool safe = false )
        {
            if( !World.Instance.Items.ContainsKey( vnum ) )
            {
                if( !safe )
                    throw new EntityException( string.Format( "No item with vnum [{0}].", vnum ) );
                _logger.WarnFormat( "Item with vnum [{0}] is not found.", vnum );
            }

            return new ItemVnum( vnum );
        }

        private static readonly ILog _logger = LogManager.GetLogger( typeof( ItemVnum ) );
    }
}
