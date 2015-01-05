from Kaerber.MUD.Entities.Aspects import IAspect

# Every Aspect file must:
#   1) be named as <aspect name/key>.py
#   2) have an Aspect class definition
#   3) have a function named construct, which cunstructs and returns an Aspect instance
#
# This is done for easy of constructing Aspects from C#, which is generally used as:
#   <aspect> is aspect name/key
# Construct <aspect>:
# import aspects.<aspect>
# myAspect = aspects.<aspect>.construct()


def construct():
    return Aspect()


class Aspect( IAspect ):
    @property
    def Host( self ):
        return self._host

    @Host.setter
    def Host( self, value ):
        self._host = value

    def Saved( self ):
        return True


    def ReceiveEvent( self, event ):
        handler = getattr( self, event.Name, None )
        if( callable( handler ) ):
            event.ReturnValue = handler( event )

        return event.ReturnValue

    def Clone( self ):
        pass

    # editor support
    def IsEditable( self ):
        return True

    def DefaultValue( self ):
        return None

    def GetMember( self, key ):
        return None

    def SetMember( self, key, value ):
        pass

    def SetValue( self, value, view ):
        pass

    def AddMember( self, value, view ):
        pass

    def RemoveMember( self, value, view ):
        pass

    def RenderInEditor( self, view, level ):
        pass

    def DescribeInEditor( self, view, level ):
        pass
