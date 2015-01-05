using System;
using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Entities.Aspects {
    public class AspectFactory {
        public static dynamic Complex() {
            Contract.Ensures( Contract.Result<object>() != null );
            return Aspect( "complex" );
        }

        public static dynamic Health() {
            return Aspect( "health" );
        }

        public static dynamic Mana() {
            return Aspect( "mana" );
        }

        public static dynamic Combat() {
            return Aspect( "combat" );
        }

        public static dynamic Movement() {
            return Aspect( "movement" );
        }

        public static dynamic AI() {
            return Aspect( "ai" );
        }

        public static dynamic Stats() {
            return Aspect( "stats" );
        }

        public static dynamic Money() {
            return Aspect( "money" );
        }

        public static dynamic Test() {
            return Aspect( "test" );
        }

        public static dynamic Weapon() {
            return Aspect( "weapon" );
        }

        public static dynamic View() {
            return Aspect( "view" );
        }

        public static dynamic Attack() {
            return Combat().InitiateAttack();
        }

        public static dynamic StatQuery() {
            return _statQueryFactory.Execute();
        }

        private static dynamic Aspect( string name ) {
            Contract.Ensures( Contract.Result<object>() != null );

            var aspect = CreateBuilder( name ).Execute();
            if( aspect == null )
                throw new InvalidOperationException( string.Format( "No such Aspect: {0}.", name ) );

            return aspect;
        }

        private static MLFunction CreateBuilder( string name ) {
            return new MLFunction {
                Code = string.Format(
                    "import aspects.{0} \n" +
                    "ret_val = aspects.{0}.construct() \n",
                    name )
            };
        }

        private static readonly MLFunction _statQueryFactory = new MLFunction {
            Code = "import game.stats \n" +
                   "ret_val = game.stats.Query() \n"
        };
    }
}
