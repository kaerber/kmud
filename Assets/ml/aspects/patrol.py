from aspect import Aspect
import random
from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return PatrolAspect()


class PatrolAspect( Aspect ):
    def __init__( self ):
        pass

    def set_Host( self, value ):
        Aspect.Host.__set__( self, value )
        self.AddNextWanderUpdate()

    # Save/Load
    def Serialize( self ):
        return {}

    def Deserialize( self, data ):
        pass

    def AddNextWanderUpdate( self ):
        nextTime = 12*World.TimeRound
        self.Host.UpdateQueue.AddRelative(
            random.randint( nextTime/2, nextTime ),
            lambda: self.Host.InRoom.ReceiveEvent(
                Event.Create( "ch_wanders", EventReturnMethod.None, { 'ch': self.Host } ) ) )

        
    # Events
    def ch_wanders( self, event ):
        if( self.Host != event['ch'] or self.Host.Combat.Fighting != None or self.Host.Flags.HasFlag( MobFlags.Sentinel ) ):
            return
        self.AddNextWanderUpdate()

        exits = filter( lambda e: not self.Host.Flags.HasFlag( MobFlags.StayArea ) or e.To.Area == self.Host.InRoom.Area,
            self.Host.InRoom.Exits )
        if( len( exits ) == 0 ): return
        
        exit = random.choice( exits )
        self.Host.Aspects.Movement.GoThroughExit( event['ch'].Room, exit )
        
