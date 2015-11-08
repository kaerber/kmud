from Kaerber.MUD.Entities import IAbility, EventReturnMethod, Event
from Newtonsoft.Json.Linq import JValue

from state import state

class ability( IAbility ):
    """character ability"""

    def __init__( self ):
        self.eventSink = lambda e: None
        self.state = state()

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

    def GetState( self ):
        return None

    def SetState( self, state ):
        for item in state:
            setattr( self.state, item.Name, JValue.Value.GetValue( item.First ) )

    def query_actions( self, e ):
        for actionName in self.actions.Keys:
            e['actions'][actionName] = self.actions[actionName]