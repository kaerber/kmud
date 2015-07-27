from aspect import Aspect

from Kaerber.MUD.Entities import Event, EventReturnMethod

# See aspect.py for details
def construct():
    return ManaAspect()


class ManaAspect( Aspect ):
    def __init__( self ):
        self.Value = 0
        self._host = None

    def Clone( self ):
        return ManaAspect()


    @property
    def Max( self ):
        e = Event.Create( "query_max_mana", EventReturnMethod.Sum )
        self.Host.ReceiveEvent( e )
        return( e.ReturnValue )
    
    # Save/Load
    def Serialize( self ):
        return { 'Value': self.Value }

    def Deserialize( self, data ):
        self.Value = data['Value']
        return self

    def Restore( self ):
        self.Value = self.Max


    # Event handlers
    def tick( self, event ):
        self.Value = min( self.Value + 1, self.Max )
