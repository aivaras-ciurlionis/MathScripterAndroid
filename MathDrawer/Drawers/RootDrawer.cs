using System.Collections.Generic;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Drawers
{
    public class RootDrawer : IDrawer
    {
        public IList<DrawableExpression> DrawExpression(IExpression expression, TextParameters p, EquationBounds bounds)
        {
            throw new System.NotImplementedException();
        }

        public EquationBounds GetBounds(IExpression expression, TextParameters p)
        {
            throw new System.NotImplementedException();
        }
    }
}