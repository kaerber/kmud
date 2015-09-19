from Kaerber.MUD.Entities import IAbility, EventReturnMethod, Event

class ability( IAbility ):
    """character ability"""

    def __init__( self ):
        self.eventSink = lambda e: None

    @property
    def EventSink( self ):
        return self.eventSink

    @EventSink.setter
    def EventSink( self, sink ):
        self.eventSink = sink


    @property
    def Actions( self ):
        return self.actions

    @Actions.setter
    def Actions( self, actions ):
        self.actions = actions


    def ReceiveEvent( self, event ):
        handler = getattr( self, event.Name, None )
        if( callable( handler ) ):
            event.ReturnValue = handler( event )

        return event.ReturnValue

    def Event( self, name, returnMethod = EventReturnMethod.None, **kwargs ):
        self.EventSink( Event.Create( name, returnMethod, kwargs ) )

