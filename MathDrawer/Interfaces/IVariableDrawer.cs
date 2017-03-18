using System.Collections.Generic;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IVariableDrawer
    {
        EquationBounds GetVariableBounds(IVariable v, TextParameters p);
        IEnumerable<DrawableElement> DrawVariable(IVariable v, int positionX, int positionY, TextParameters p);
    }
}