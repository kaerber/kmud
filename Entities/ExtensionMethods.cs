using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaerber.MUD.Entities
{
    public static class ExtensionMethods
    {
        public static string Unquote( this string src )
        {
            return (
                ( ( src.StartsWith( "'" ) && src.EndsWith( "'" ) ) || ( src.StartsWith( "\"" ) && src.EndsWith( "\"" ) ) )
                ? src.Substring( 1, src.Length - 2 )
                : src );
        }

        // Dictionary<T1, T2> Dictionary<T1, T2>.AddIf( T1 key, T2 value, bool condition )
        public static IDictionary<T1, T2> AddIf<T1, T2>( this IDictionary<T1, T2> store, T1 key, T2 value, bool condition )
        {
            if( condition )
                store.Add( key, value );
            return( store );
        }

        // Dictionary<T1, T2> Dictionary<T1, T2>.AddEx( T1 key, T2 value )
        public static IDictionary<T1, T2> AddEx<T1, T2>( this IDictionary<T1, T2> store, T1 key, T2 value )
        {
            store.Add( key, value );
            return( store );
        }


        public static List<T1> AddEx<T1>( this List<T1> store, T1 item )
        {
            store.Add( item );
            return ( store );
        }


        public static string Match( this IEnumerable<string> host, string key, string def = null )
        {
            if( string.IsNullOrWhiteSpace( key ) )
                return ( def );

            var test1 = host.FirstOrDefault( item => item.Equals( key, StringComparison.InvariantCultureIgnoreCase ) );
            var test2 = host.FirstOrDefault( item => item.StartsWith( key, StringComparison.InvariantCultureIgnoreCase ) );
            return (
                host.FirstOrDefault( item => item.Equals( key, StringComparison.InvariantCultureIgnoreCase ) )
                ??  host.FirstOrDefault( item => item.StartsWith( key, StringComparison.InvariantCultureIgnoreCase ) )
                ?? def
            );
        }


        public static Func<T> NotNull<T>( this Func<T> func ) where T : new()
        {
            return( func ?? ( () => new T() ) );
        }
    }
}
