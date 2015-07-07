from aspect import Aspect

from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return HealthAspect()


class HealthAspect( Aspect ):
    def __init__( self ):
        self.Wounds = 0


    def Clone( self ):
        return HealthAspect()


    @property
    def Max( self ):
        assert( self.Host != None )

        e = Event.Create( 'query_max_health', EventReturnMethod.Sum )
        self.Host.ReceiveEvent( e )
        return e.ReturnValue


    @property
    def Condition( self ):
        return 100*( self.Max - self.Wounds )//self.Max

    # Save/Load
    def Serialize( self ):
        return { 'Wounds': self.Wounds }

    def Deserialize( self, data ):
        if( 'Wounds' in data ):
            self.Wounds = data['Wounds']
        else:
            self.Restore()

        return self


    def GainHealth( self, health ):
        self.Wounds = max( self.Wounds - health, 0 )

    def Restore( self ):
        self.Wounds = 0
    

    # Event handlers
    def tick( self, event ):
        self.GainHealth( 1 )

        
    def ch_dealt_damage_to_this( self, e ):
        print e["this"].ShortDescr, "damage", e["damage"]
        self.Wounds += e["damage"]
        print e["this"].ShortDescr, "wounds", self.Wounds
        if self.Wounds > self.Max:
            self.Host.SetTimedEvent( 0, lambda: self.Host.Die() )
