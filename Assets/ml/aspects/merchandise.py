import sys

import clr

from mud import *
from aspect import Aspect

from Kaerber.MUD.Entities import *

# See aspect.py for details
def construct():
    return MerchandiseAspect()

class MerchandiseAspect:
    def Saved( self ):
        return False

    # events
    def shop_has_item( self, event ):
        return self.Host.Vnum == event["vnum"]

