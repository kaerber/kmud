using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaerber.MUD.Entities
{
    public class AffectSet : IList<Affect>
    {
        private readonly List<Affect> _list;
        private Entity _host;

        public AffectSet()
        {
            _list = new List<Affect>();
        }

        public AffectSet( IEnumerable<Affect> source, Entity host )
        {
            _list = new List<Affect>( source );
            _host = host;
        }

        #region IList members
        public Affect this[int index]
        {
            get { return ( _list[index] ); }
            set
            {
                throw new NotSupportedException( "Cannot directly set affects. Use add/remove instead." );
            }
        }

        public int Count
        {
            get { return ( _list.Count ); }
        }

        public void Add( Affect item )
        {
            if( !CheckAffectEligibility( item ) )
                throw new EntityException(
                    string.Format(
                        "Affect target {0} is not legal for affect {1}.",
                        _host.GetType(),
                        item.Name )
                );

            _list.Add( item );
        }

        public void Insert( int index, Affect item )
        {
            if( !CheckAffectEligibility( item ) )
            {
                throw new EntityException(
                    string.Format(
                        "Affect target {0} is not legal for affect {1}.",
                        _host.GetType(),
                        item.Name
                    )
                );
            }
                        
            _list.Insert( index, item );
        }

        public Affect Cast( string name, long duration )
        {
            var data = AffectInfo.Get( name );
            if( Contains( name ) && !data.Flags.HasFlag( AffectFlags.Multiple ) )
                return ( null );

            var affect = new Affect( data ) { Duration = duration };
            Add( affect );
            affect.ReceiveEvent( Event.Create( "affect_cast", EventReturnMethod.None ), _host );
            return ( affect );
        }

        public void Clear( string name )
        {
            _list.FindAll( a => a.Name == name ).ForEach( a => _list.Remove( a ) );
        }

        public bool Remove( Affect item )
        {
            return ( _list.Remove( item ) );
        }

        public void RemoveAt( int index )
        {
            _list.RemoveAt( index );
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains( Affect item )
        {
            return ( _list.Contains( item ) );
        }

        public bool Contains( string name )
        {
            return ( _list.Exists( aff => aff.Name == name ) );
        }

        public int IndexOf( Affect item )
        {
            return ( _list.IndexOf( item ) );
        }

        public void CopyTo( Affect[] array, int arrayIndex )
        {
            _list.CopyTo( array, arrayIndex );
        }

        public bool IsReadOnly
        {
            get { return ( false ); }
        }

        public IEnumerator<Affect> GetEnumerator()
        {
            return ( _list.GetEnumerator() );
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ( GetEnumerator() );
        }
        #endregion

        public void Update()
        {
            _list
                .Where( affect => !affect.IsCurrent )
                .ToList()
                .ForEach( affect => _list.Remove( affect ) );
        }

        public void SetHost( Entity host )
        {
            _host = host;
        }

        public void ReceiveEvent( Event e )
        {
            _list.ForEach( affect => affect.ReceiveEvent( e, _host ) );
        }

        private bool CheckAffectEligibility( Affect affect )
        {
            return (
                ( _host is Room && affect.Target == AffectTarget.Room )
                || ( _host is Item && affect.Target == AffectTarget.Item )
                || ( _host is Character && affect.Target == AffectTarget.Character ) );
        }

        public Affect this[string name]
        {
            get { return ( _list.Find( aff => aff.Name == name ) ); }
        }

        public List<Affect> Get( string name )
        {
            return ( _list.FindAll( aff => aff.Name == name ) );
        }
    }
}
