using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Entities
{
    public class ActionQueueSet
    {
        protected Dictionary<string, ActionQueue> Set;


        public ActionQueueSet()
        {
            Set = new Dictionary<string, ActionQueue>();
        }

        public ActionQueueSet( Character host )
        {
            Contract.Requires( host != null );

            Set = new Dictionary<string, ActionQueue>
            {
                { "autoattack", new ActionQueue( "autoattack", host ) }
            };
        }


        public ActionQueue this[string index]
        {
            get { return Set[index]; }
        }

        public virtual void EnqueueAction( string queue, CharacterAction action )
        {
            Set[queue].Add( action );
        }
    }
}
