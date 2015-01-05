from aspect import Aspect
from complex import ComplexAspect
from patrol import PatrolAspect

from Kaerber.MUD.Entities import *

# See aspect.py for details
def construct():
    return AIAspect()


class AIAspect( ComplexAspect ):
    def __init__( self ):
        super( AIAspect, self ).__init__()
        self.patrol = PatrolAspect()

    # Save/Load
    def Saved( self ):
        return False

    def Serialize( self ):
        return {}

    def Deserialize( self, data ):
        pass