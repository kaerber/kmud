class Query( object ):
    def __init__( self ):
        self.con = 0
        self.int = 0
        self.Wounds = 0
        self.ManaSpent = 0
    
    @property    
    def Con( self ):
        return self.con

    @Con.setter
    def Con( self, con ):
        self.con += con

    @property
    def Int( self ):
        return self.int

    @Int.setter
    def Int( self, value ):
        self.int += value

    @property
    def HP( self ):
        return self.MaxHP - self.Wounds

    @property
    def MaxHP( self ):
        return self.Con*25

    @property
    def MP( self ):
        return self.MaxMP - self.ManaSpent

    @property
    def MaxMP( self ):
        return self.Int*25
