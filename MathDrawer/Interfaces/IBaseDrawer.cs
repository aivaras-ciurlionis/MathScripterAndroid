using System.Collections.Generic;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IBaseDrawer
    {
        IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters parameters, EquationBounds bounds);
        EquationBounds GetBounds(IExpression expression, TextParameters parameters);
    }
}