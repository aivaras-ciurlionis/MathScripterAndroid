using MathDrawer.Drawers;
using MathDrawer.Interfaces;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathDrawer
{
    public class DrawerFactory : IDrawerFactory
    {
        public IDrawer GetDrawer(IExpression expression)
        {
            if (expression is DivisionExpression)
            {
                return new FractionDrawer();
            }

            if (expression is ExponentExpression)
            {
                return new SuperscriptDrawer();
            }

            if (expression is Monomial)
            {
                return new MonomialDrawer();
            }

            if (expression is ParenthesisExpression)
            {
                return new ParenthesisDrawer();
            }
            
            switch (expression.Arity)
            {
                case 2: return new BinaryDrawer();
                case 1: return new UnaryDrawer();
                default: return new BinaryDrawer();
            }

        }
    }
}
