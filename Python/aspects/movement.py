import random

from aspect import Aspect

from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return MovementAspect()


class MovementAspect( Aspect ):
    def __init__( self ):
        self._host = None
        self._in = None

    def Clone( self ):
        return MovementAspect()
    

    def Saved( self ):
        return False

    # Save/Load
    def Serialize( self ):
        return {}

    def Deserialize( self, data ):
        return self

    def CanLeaveRoom( self, room ):
        return self.Host.Can( 'leave_room', { 'room': room } )

    def CanEnterRoom( self, room ):
        event = Event.Create( 'ch_can_enter_room', EventReturnMethod.And, { 'ch': self.Host, 'room': room } )
        room.ReceiveEvent( event )
        self.Host.ReceiveEvent( event )
        return event.ReturnValue

    
    def WentFromRoom( self, fromroom, toroom ):
        event = Event.Create( 'ch_went_from_room_to_room', 
                              EventReturnMethod.None,
                              { 'ch': self.Host,
                                'room_from': fromroom, 
                                'room_to': toroom,
                                'exit': fromroom.Exits[toroom], 
                                'entrance': toroom.Exits[fromroom] } )
        fromroom.ReceiveEvent( event )
        toroom.ReceiveEvent( event )

    def LeftRoom( self, room ):
        event = Event.Create( 'ch_left_room', EventReturnMethod.None, { 'ch': self.Host, 'room': room } )
        room.ReceiveEvent( event )
        self.Host.ReceiveEvent( event )

    def EnteredRoom( self, room ):
        self.Host.Has( "entered_room", { 'room': room } )