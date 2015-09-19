from abilities.ability import ability

class movement( ability ):
    """description of class"""
    def test_event( self, e ):
        print __file__
        self.Event( "response_event", response="Arrrr!", test="testy" )

result = movement()

