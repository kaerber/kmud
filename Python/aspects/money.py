from Kaerber.MUD.Entities import *
from aspect import Aspect
import currency

# See aspect.py for details
def construct():
    return MoneyAspect()

class MoneyAspect( Aspect ):
    def Coins( self ):
        return [ item for item in self.Host.Inventory if item.Flags.HasFlag( ItemFlags.Money ) ]

    def Value( self ):
        return sum( [ coin.TotalValue for coin in self.Coins() ] )


    # events
    def this_will_pay_money( self, event ):
        return True

    def this_can_pay_money( self, event ):
        if( self.Value() >= event["amount"] ):
            return True
        self.Host.Event( "this_couldnt_afford_amount", 
                         EventReturnMethod.None, 
                         { "this": self.Host, "amount": event["amount"] } )
        return False


    def this_pays_money( self, event ):
        ( coinsToPay, change ) = self.DetermineMoneyToPay( event.Parameters["amount"] )
        coinsChange = self.DetermineChangeToReturn( change, coinsToPay )

        event["coins"] = coinsToPay
        for coin in coinsToPay:
            self.Host.Inventory.Remove( coin.Vnum, coin.Count )

        event["change"] = coinsChange
        for coin in coinsChange:
            self.Host.Inventory.Add( coin )


    def DetermineMoneyToPay( self, money ):
        coins = sorted( self.Coins() , key = lambda item : item.Cost )
        coinsPaid = Aspects.ItemSet()
        for coin in coins:
            coinPaid = Item( coin )

            if( coinPaid.Stack != None ):
                if( coin.TotalValue <= money ):
                    coinPaid.Stack.Count = coin.Stack.Count
                else:
                    coinPaid.Stack.Count = math.ceil( float( money )/coinPaid.Cost )

            coinsPaid.Add( coinPaid )
            money -= CoinValue( coinPaid )
        
            if( money <= 0 ): break
    
        return( coinsPaid, -money )


    def DetermineChangeToReturn( self, change, coinsPaid ):
        coinsChange = Aspects.ItemSet()
        if( change <= 0 ):
            return( coinsChange )

        for coinChange in currency.GetCurrencies():
            if( coinChange.Cost > change ): continue
        
            ( count, change ) = divmod( change, coinChange.Cost )
            countPaidDiff = min( count, coinsPaid.ItemCount( coinChange.Vnum ) )
            if( countPaidDiff > 0 ):
                count -= countPaidDiff
                coinsPaid.Remove( coinChange.Vnum, countPaidDiff )
        
            if( coinChange.Stack.MaxCount > 0 ):
                while( count >= coinChange.Stack.MaxCount ):
                    fullStack = Item( coinChange )
                    fullStack.Stack.Count = fullStack.Stack.MaxCount
                    coinsChange.Add( fullStack )
                    count -= fullStack.Stack.Count
            
            if( count > 0 ):
                coinChange.Stack.Count = count
                coinsChange.Add( coinChange )
    
        return( coinsChange )
