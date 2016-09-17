from Kaerber.MUD.Entities import EventReturnMethod

from abilities.ability import ability

class movement( ability ):
    """movement ability"""
    def test_event( self, e ):
        print __file__
        self.Event( "response_event", response="Arrrr!", test="testy" )

    def this_loaded( self, e ):
        room = self.Event( "world:query_room_by_id", EventReturnMethod.First, id=self.state.location )
        print( room )

result = movement()

