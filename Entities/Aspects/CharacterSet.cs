using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kaerber.MUD.Entities.Aspects {
    public class CharacterSet : IList<Character> {
        public CharacterSet() {
            _list = new List<Character>();
        }

        public CharacterSet( IEnumerable<Character> source ) {
            _list = new List<Character>( source );
        }

        public CharacterSet Where( Predicate<Character> predicate ) {
            return new CharacterSet( _list.Where( ch => predicate( ch ) ) );
        }


        public Character Find( Func<Character, bool> predicate ) {
            return this.FirstOrDefault( predicate );
        }

        public Character Find( string partialName, Func<Character, bool> filter = null ) {
            return Find( ch => ch.MatchNames( partialName ) && ( filter == null || filter( ch ) ) );
        }

        public Character LoadMob( string vnum ) {
            var mob = Character.CreateMob( vnum );
            mob.SetRoom( Host );
            return mob;
        }

        private readonly List<Character> _list;


        public Room Host;

        #region Implementation of IEnumerable

        public IEnumerator<Character> GetEnumerator() {
            return _list.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Character>

        public virtual void Add( Character ch ) {
            _list.Add( ch );
        }


        public void Clear() {
            _list.Clear();
        }


        public bool Contains( Character ch ) {
            return _list.Contains( ch );
        }

        public void CopyTo( Character[] array, int arrayIndex ) {
            _list.CopyTo( array, arrayIndex );
        }

        public virtual bool Remove( Character ch ) {
            return _list.Remove( ch );
        }

        public virtual int Count => _list.Count;

        public bool IsReadOnly => false;

        #endregion

        #region Implementation of IList<Character>

        public virtual int IndexOf( Character ch ) {
            return _list.IndexOf( ch );
        }


        public void Insert( int index, Character ch ) {
            _list.Insert( index, ch );
        }


        public void RemoveAt( int index ) {
            _list.RemoveAt( index );
        }


        public virtual Character this[ int index ] {
            get { return _list[index]; }
            set { throw new NotSupportedException( "You should use Add instead." ); }
        }

        #endregion
    }
}