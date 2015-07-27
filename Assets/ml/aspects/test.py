from aspect import Aspect

from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return TestAspect()


class TestAspect( Aspect ):
    def test_complete( self, event ):
        event.ReturnValue = True