
def GetCurrencies():
    return( sorted( map( Item.Create, ( 'heaven_gold', 'heaven_silver' ) ),
        key = lambda item : -item.Cost ) )
