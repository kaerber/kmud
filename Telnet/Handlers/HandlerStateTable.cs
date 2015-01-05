namespace Kaerber.MUD.Telnet.Handlers {
    public enum State {
        No,
        Yes,
        WantNoEmpty,
        WantNoOpposite,
        WantYesEmpty,
        WantYesOpposite
    }

    public enum CommandDirection {
        Sent = 0,
        Received = 4
    }

    public enum HandlerStateInput {
        SentDo,
        SentDont,
        SentWill,
        SentWont,
        ReceivedDo,
        ReceivedDont,
        ReceivedWill,
        ReceivedWont
    }

    public class HandlerStateTable {
        private HandlerStateTable() {}

        private const State No = State.No;
        private const State Yes = State.Yes;
        private const State WantNoEmpty = State.WantNoEmpty;
        private const State WantNoOpposite = State.WantNoOpposite;
        private const State WantYesEmpty = State.WantYesEmpty;
        private const State WantYesOpposite = State.WantYesOpposite;

        protected static readonly Command Do = Command.Do;
        protected static readonly Command Dont = Command.Dont;
        protected static readonly Command Will = Command.Will;
        protected static readonly Command Wont = Command.Wont;

        public static Transition[,] LocalQYesMethodTable = {
        //                      sDo   sDont sWill                      sWont                       rDo                       rDont                  rWill rWont
        /* No */              { null, null, _( WantYesEmpty, Will )  , null                      , _( Yes, Will )        , null                 , null, null },
        /* Yes */             { null, null, null                     , _( WantNoEmpty, Wont )    , null                  , _( No, Wont )        , null, null },
        /* WantNoEmpty */     { null, null, _( WantNoOpposite, null ), null                      , _( WantNoEmpty, null ), _( No, null )        , null, null },
        /* WantNoOpposite */  { null, null, null                     , _( WantNoEmpty, null )    , null                  , _( WantYesEmpty, Do ), null, null },
        /* WantYesEmpty */    { null, null, null                     , _( WantYesOpposite, null ), _( Yes, null )        , _( No, null )        , null, null },
        /* WantYesOpposite */ { null, null, _( WantYesEmpty, null )  , null                      , _( WantNoEmpty, Dont ), _( No, null )        , null, null }
        };

        public static Transition[,] RemoteQYesMethodTable = {
        //                      sDo                        sDont                       sWill sWont rDo   rDont rWill                   rWont
        /* No */              { _( WantYesEmpty, Do ),     null,                       null, null, null, null, _( Yes, Do ),           null                  },
        /* Yes */             { null,                      _( WantNoEmpty, Dont ),     null, null, null, null, null,                   _( No, Dont )         },
        /* WantNoEmpty */     { _( WantNoOpposite, null ), null,                       null, null, null, null, _( No, null ),          _( No, null )         },
        /* WantNoOpposite */  { null,                      _( WantNoEmpty, null ),     null, null, null, null, _( Yes, null ),         _( WantYesEmpty, Do ) },
        /* WantYesEmpty */    { null,                      _( WantYesOpposite, null ), null, null, null, null, _( Yes, null ),         _( No, null )         },
        /* WantYesOpposite */ { _( WantYesEmpty, null ),   null,                       null, null, null, null, _( WantNoEmpty, Dont ), _( No, null )         }
        };

        public static Transition[,] LocalQNoMethodTable = {
        //                      sDo   sDont sWill                      sWont                       rDo                       rDont                  rWill rWont
        /* No */              { null, null, _( WantYesEmpty, Will )  , null                      , _( Yes, Will )        , null                 , null, null },
        /* Yes */             { null, null, null                     , _( WantNoEmpty, Wont )    , null                  , _( No, Wont )        , null, null },
        /* WantNoEmpty */     { null, null, _( WantNoOpposite, null ), null                      , _( WantNoEmpty, null ), _( No, null )        , null, null },
        /* WantNoOpposite */  { null, null, null                     , _( WantNoEmpty, null )    , null                  , _( WantYesEmpty, Do ), null, null },
        /* WantYesEmpty */    { null, null, null                     , _( WantYesOpposite, null ), _( Yes, null )        , _( No, null )        , null, null },
        /* WantYesOpposite */ { null, null, _( WantYesEmpty, null )  , null                      , _( WantNoEmpty, Dont ), _( No, null )        , null, null }
        };

        public static Transition[,] RemoteQNoMethodTable = {
        //                      sDo                        sDont                       sWill sWont rDo   rDont rWill                   rWont
        /* No */              { null,                      null,                       null, null, null, null, _( No, Dont ),          null                  },
        /* Yes */             { null,                      _( WantNoEmpty, Dont ),     null, null, null, null, null,                   _( No, Dont )         },
        /* WantNoEmpty */     { null,                      null,                       null, null, null, null, _( No, null ),          _( No, null )         },
        /* WantNoOpposite */  { null,                      _( WantNoEmpty, null ),     null, null, null, null, _( Yes, null ),         _( WantYesEmpty, Do ) },
        /* WantYesEmpty */    { null,                      _( WantYesOpposite, null ), null, null, null, null, _( Yes, null ),         _( No, null )         },
        /* WantYesOpposite */ { null,                      null,                       null, null, null, null, _( WantNoEmpty, Dont ), _( No, null )         }
        };

        private static Transition _( State state, Command command ) {
            return new Transition( state, command );
        }
    }
}
