using MathDrawer.Drawers;
using MathDrawer.Helpers;
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
            var factory = new DrawerFactory();
            var measurer = new BoundsMeasurer(factory);
            var textMeasurer = new TextMeasurer();
            var variableDrawer = new VariableDrawer(textMeasurer);

            if (expression is DivisionExpression)
            {
                return new FractionDrawer(factory, measurer);
            }

            if (expression is ExponentExpression)
            {
                return new SuperscriptDrawer(factory, measurer);
            }

            if (expression is Monomial)
            {
                return new MonomialDrawer(textMeasurer, variableDrawer);
            }

            if (expression is ParenthesisExpression)
            {
                return new ParenthesisDrawer(factory, measurer, textMeasurer);
            }

            switch (expression.Arity)
            {
                case 2: return new BinaryDrawer(factory, measurer, textMeasurer);
                case 1: return new UnaryDrawer();
                default: return new BinaryDrawer(factory, measurer, textMeasurer);
            }

        }
    }
}
