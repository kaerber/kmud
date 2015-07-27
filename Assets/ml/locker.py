from System.Threading import Monitor

class Locker( object ):
    def __init__( self, obj ):
        self.obj = obj

    def __enter__( self ):
        Monitor.Enter( self.obj )

    def __exit__( self, exc_type, exc_value, exc_tb ):
        Monitor.Exit( self.obj )