using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Helpers
{
    public class BoundsMeasurer : IBoundsMeasurer
    {
        private readonly IDrawerFactory _drawerFactory;

        public BoundsMeasurer(IDrawerFactory drawerFactory)
        {
            _drawerFactory = drawerFactory;
        }

        public EquationBounds GetOperandBounds(IExpression operand, TextParameters paint)
        {
            return _drawerFactory.GetDrawer(operand)
                .GetBounds(operand, paint);
        }
    }
}