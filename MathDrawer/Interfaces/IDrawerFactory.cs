using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IDrawerFactory
    {
        IDrawer GetDrawer(IExpression expression);
    }
}